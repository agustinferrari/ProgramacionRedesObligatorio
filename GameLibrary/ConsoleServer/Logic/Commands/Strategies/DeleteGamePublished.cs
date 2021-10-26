using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;
using System.Threading.Tasks;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class DeleteGamePublished : CommandStrategy
    {

        public override async Task HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            string gameName = await clientNetworkStreamHandler.ReceiveString(header.IDataLength);
            string responseMessage;
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
            {
                string userName = _clientHandler.GetUsername(clientNetworkStreamHandler);
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
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListOwnedGames, responseMessage);

        }
    }
}
