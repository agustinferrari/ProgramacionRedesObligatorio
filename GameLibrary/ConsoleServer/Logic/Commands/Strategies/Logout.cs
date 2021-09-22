using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class Logout : CommandStrategy
    {

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
        {
            if (_clientHandler.IsSocketInUse(clientSocketHandler))
                _clientHandler.RemoveClient(clientSocketHandler);
            string responseMessageResult = ResponseConstants.LogoutSuccess;
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.Logout, responseMessageResult);
        }
    }
}
