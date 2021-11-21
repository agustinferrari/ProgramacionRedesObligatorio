using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System.Threading.Tasks;
using LogsModels;
using Server.Domain;
using Server.Utils.CustomExceptions;

namespace Server.Logic.Commands.Strategies
{
    public class DeleteGame : CommandStrategy
    {

        public override async Task<LogGameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            LogGameModel log = new LogGameModel(header.ICommand);
            string gameName = await clientNetworkStreamHandler.ReceiveString(header.IDataLength);
            log.Game = gameName;
            string responseMessage;
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
            {
                string userName = _clientHandler.GetUsername(clientNetworkStreamHandler);
                log.User = userName;
                try
                {
                    User user = _userController.GetUser(userName);
                    Game gameToDelete = _gameController.GetCertainGamePublishedByUser(user, gameName);
                    if (gameToDelete != null)
                    {
                        _gameController.DeletePublishedGameByUser(gameToDelete);
                        responseMessage = ResponseConstants.DeleteGameSuccess;
                        log.Result = true;
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
            return log;
        }
    }
}
