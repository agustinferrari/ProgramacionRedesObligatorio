using Common.NetworkUtils;
using Common.Protocol;
using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class BuyGame : CommandStrategy
    {

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
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
                catch (InvalidUsernameException e)
                {
                    responseMessageResult = ResponseConstants.InvalidUsernameError;
                }
                catch (InvalidGameException e)
                {
                    responseMessageResult = ResponseConstants.InvalidGameError;
                }
                catch (GameAlreadyBoughtException ge)
                {
                    responseMessageResult = ResponseConstants.GameAlreadyBought;
                }
            }
            else
            {
                responseMessageResult = ResponseConstants.AuthenticationError;
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.BuyGame, responseMessageResult);
        }
    }
}
