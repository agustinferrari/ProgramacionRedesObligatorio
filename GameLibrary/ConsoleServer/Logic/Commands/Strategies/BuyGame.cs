using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Utils.CustomExceptions;


namespace ConsoleServer.Logic.Commands.Strategies
{
    public class BuyGame : CommandStrategy
    {

        public override void HandleRequest(Header header, ISocketHandler clientSocketHandler)
        {
            string gameName = clientSocketHandler.ReceiveString(header.IDataLength);
            string username;
            string responseMessageResult;
            if (_clientHandler.IsSocketInUse(clientSocketHandler))
            {
                username = _clientHandler.GetUsername(clientSocketHandler);
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
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.BuyGame, responseMessageResult);
        }
    }
}
