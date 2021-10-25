using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class Login : CommandStrategy
    {

        public override void HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            string userName = clientNetworkStreamHandler.ReceiveString(header.IDataLength).Result;
            string responseMessageResult;
            if (_clientHandler.IsClientLogged(userName))
                responseMessageResult = ResponseConstants.LoginErrorAlreadyLogged;
            else
            {
                if (!_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
                {
                    _clientHandler.AddClient(clientNetworkStreamHandler, userName);
                    _userController.TryAddUser(userName);
                    responseMessageResult = ResponseConstants.LoginSuccess;
                }
                else
                    responseMessageResult = ResponseConstants.LoginErrorSocketAlreadyInUse;
            }
            clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.Login, responseMessageResult);
        }
    }
}
