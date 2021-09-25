using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class ListOwnedGames : CommandStrategy
    {

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
        {
            string emptyString = "";
            string responseMessage;
            if (_clientHandler.IsSocketInUse(clientSocketHandler))
            {
                string userName = _clientHandler.GetUsername(clientSocketHandler);
                string gameList = _userController.ListOwnedGameByUser(userName);
                responseMessage = gameList;
                if (gameList == emptyString)
                    responseMessage = ResponseConstants.LibraryError;

            }
            else
                responseMessage = ResponseConstants.AuthenticationError;
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListOwnedGames, responseMessage);
        }
    }
}
