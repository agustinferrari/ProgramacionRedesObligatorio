using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Utils.CustomExceptions;


namespace ConsoleServer.Logic.Commands.Strategies
{
    public class BuyGame : CommandStrategy
    {

        public override void HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            string gameName = clientNetworkStreamHandler.ReceiveString(header.IDataLength).Result;
            string username;
            string responseMessageResult;
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
            {
                username = _clientHandler.GetUsername(clientNetworkStreamHandler);
                try
                {
                    _userController.BuyGame(username, gameName);
                    responseMessageResult = ResponseConstants.BuyGameSuccess;
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
            clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.BuyGame, responseMessageResult);
        }
    }
}
