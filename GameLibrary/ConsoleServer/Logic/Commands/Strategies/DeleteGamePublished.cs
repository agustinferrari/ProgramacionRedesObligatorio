using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class DeleteGamePublished : CommandStrategy
    {

        public override void HandleRequest(Header header, ISocketHandler clientSocketHandler)
        {
            string gameName = clientSocketHandler.ReceiveString(header.IDataLength);
            string responseMessage;
            if (_clientHandler.IsSocketInUse(clientSocketHandler))
            {
                string userName = _clientHandler.GetUsername(clientSocketHandler);
                try
                {
                    User user = _userController.GetUser(userName);
                    Game gameToDelete = _gameController.GetCertainGamePublishedByUser(user, gameName);
                    if (gameToDelete != null)
                    {
                        _gameController.DeletePublishedGameByUser(gameToDelete);
                        responseMessage = ResponseConstants.DeleteGameSuccess;
                    }
                    else
                        responseMessage = ResponseConstants.UnauthorizedGame;
                }
                catch (InvalidUsernameException)
                {
                    responseMessage = ResponseConstants.InvalidUsernameError;
                }
                catch (InvalidGameException)
                {
                    responseMessage = ResponseConstants.DeleteGameError;
                }
            }
            else
                responseMessage = ResponseConstants.AuthenticationError;
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListOwnedGames, responseMessage);

        }
    }
}
