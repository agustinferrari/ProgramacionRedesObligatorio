using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class Logout : CommandStrategy
    {

        public override async Task HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
                _clientHandler.RemoveClient(clientNetworkStreamHandler);
            string responseMessageResult = ResponseConstants.LogoutSuccess;
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.Logout, responseMessageResult);
        }
    }
}
