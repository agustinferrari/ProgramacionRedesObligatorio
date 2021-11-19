﻿using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System.Threading.Tasks;
using LogsModels;

namespace ServerGRPC.Logic.Commands.Strategies
{
    public class Logout : CommandStrategy
    {

        public override async Task<LogGameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            LogGameModel log = new LogGameModel(header.ICommand);
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
