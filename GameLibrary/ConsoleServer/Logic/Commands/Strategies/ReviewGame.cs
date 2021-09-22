using Common.NetworkUtils;
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

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
        {
            string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
            string[] gameData = rawData.Split('%');
            string gameName = gameData[0];
            string rating = gameData[1];
            string comment = gameData[2];


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
                catch (InvalidUsernameException e)
                {
                    responseMessageResult = ResponseConstants.InvalidUsernameError;
                }
                catch (InvalidGameException e)
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
