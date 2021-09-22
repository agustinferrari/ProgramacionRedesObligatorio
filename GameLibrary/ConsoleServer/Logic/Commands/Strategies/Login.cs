using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class Login : CommandStrategy
    {

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
        {
            string userName = clientSocketHandler.ReceiveString(header.IDataLength); //Podriamos hacer un metodo que haga todo esto de una
            string responseMessageResult;
            if (_clientHandler.IsClientLogged(userName))
            {
                responseMessageResult = ResponseConstants.LoginErrorAlreadyLogged;
            }
            else
            {
                if (!_clientHandler.IsSocketInUse(clientSocketHandler))
                {
                    _clientHandler.AddClient(clientSocketHandler, userName);
                    _userController.TryAddUser(userName);
                    responseMessageResult = ResponseConstants.LoginSuccess;
                }
                else
                {
                    responseMessageResult = ResponseConstants.LoginErrorSocketAlreadyInUse;
                }
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.Login, responseMessageResult);
        }
    }
}
