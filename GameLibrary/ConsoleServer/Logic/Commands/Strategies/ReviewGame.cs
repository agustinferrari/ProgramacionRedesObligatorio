using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class ReviewGame : CommandStrategy
    {
        public override void HandleRequest(Header header, ISocketHandler clientSocketHandler)
        {
            int firstElement = 0;
            int secondElement = 1;
            int thirdElement = 2;
            string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
            string[] gameData = rawData.Split('%');
            string gameName = gameData[firstElement];
            string rating = gameData[secondElement];
            string comment = gameData[thirdElement];

            string responseMessageResult;
            if (_clientHandler.IsSocketInUse(clientSocketHandler))
            {
                string userName = _clientHandler.GetUsername(clientSocketHandler);
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
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ReviewGame, responseMessageResult);
        }
    }
}
