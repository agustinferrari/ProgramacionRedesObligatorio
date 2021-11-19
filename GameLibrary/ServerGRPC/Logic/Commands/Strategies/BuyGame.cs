using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ServerGRPC.Utils.CustomExceptions;
using System.Threading.Tasks;
using LogsModels;

namespace ServerGRPC.Logic.Commands.Strategies
{
    public class BuyGame : CommandStrategy
    {

        public override async Task<LogGameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            LogGameModel log = new LogGameModel(header.ICommand);
            string gameName = await clientNetworkStreamHandler.ReceiveString(header.IDataLength);
            log.Game = gameName;
            string username;
            string responseMessageResult;
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
            {
                username = _clientHandler.GetUsername(clientNetworkStreamHandler);
                log.User = username;
                try
                {
                    _userController.BuyGame(username, gameName);
                    responseMessageResult = ResponseConstants.BuyGameSuccess;
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
                catch (GameAlreadyBoughtException)
                {
                    responseMessageResult = ResponseConstants.GameAlreadyBought;
                }
            }
            else
                responseMessageResult = ResponseConstants.AuthenticationError;
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.BuyGame, responseMessageResult);
            return log;
        }
    }
}
