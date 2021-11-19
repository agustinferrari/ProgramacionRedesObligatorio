using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ServerGRPC.Domain;
using ServerGRPC.Utils.CustomExceptions;
using System;
using System.Threading.Tasks;
using LogsModels;

namespace ServerGRPC.Logic.Commands.Strategies
{
    public class ReviewGame : CommandStrategy
    {
        public override async Task<LogGameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            LogGameModel log = new LogGameModel(header.ICommand);
            int firstElement = 0;
            int secondElement = 1;
            int thirdElement = 2;
            string rawData = await clientNetworkStreamHandler.ReceiveString(header.IDataLength);
            string[] gameData = rawData.Split('%');
            string gameName = gameData[firstElement];
            string rating = gameData[secondElement];
            string comment = gameData[thirdElement];
            log.Game = gameName;

            string responseMessageResult;
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
            {
                string userName = _clientHandler.GetUsername(clientNetworkStreamHandler);
                log.User = userName;
                try
                {
                    Review newReview = new Review
                    {
                        User = _userController.GetUser(userName),
                        Comment = comment,
                        Rating = Int32.Parse(rating),
                    };

                    _gameController.AddReview(gameName, newReview);
                    responseMessageResult = ResponseConstants.ReviewGameSuccess;
                    log.Result = true;
                }
                catch (InvalidUsernameException)
                {
                    responseMessageResult = ResponseConstants.InvalidUsernameError;
                }
                catch (InvalidGameException)
                {
                    responseMessageResult = ResponseConstants.InvalidGameError;
                }
                catch (Exception e) when (e is FormatException || e is InvalidReviewRatingException)
                {
                    responseMessageResult = ResponseConstants.InvalidRatingException;
                }
            }
            else
                responseMessageResult = ResponseConstants.AuthenticationError;
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.ReviewGame, responseMessageResult);
            return log;
        }
    }
}
