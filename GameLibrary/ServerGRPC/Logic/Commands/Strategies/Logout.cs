using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using CommonModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerGRPC.Logic.Commands.Strategies
{
    public class Logout : CommandStrategy
    {

        public override async Task<GameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            GameModel log = new GameModel(header.ICommand);
            string userName = _clientHandler.GetUsername(clientNetworkStreamHandler);
            log.User = userName;
            if (userName != "")
                _clientHandler.RemoveClient(clientNetworkStreamHandler);
            log.Result = true;
            string responseMessageResult = ResponseConstants.LogoutSuccess;
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.Logout, responseMessageResult);
            return log;
        }
    }
}
