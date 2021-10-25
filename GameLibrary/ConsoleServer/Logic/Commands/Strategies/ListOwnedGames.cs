using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class ListOwnedGames : CommandStrategy
    {

        public override void HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            string emptyString = "";
            string responseMessage;
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
            {
                string userName = _clientHandler.GetUsername(clientNetworkStreamHandler);
                string gameList = _userController.ListOwnedGameByUser(userName);
                responseMessage = gameList;
                if (gameList == emptyString)
                    responseMessage = ResponseConstants.LibraryError;

            }
            else
                responseMessage = ResponseConstants.AuthenticationError;
            clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListOwnedGames, responseMessage);
        }
    }
}
