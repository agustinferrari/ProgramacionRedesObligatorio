using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class ListGames : CommandStrategy
    {

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
        {
            string gameList = _gameController.GetGames();
            string responseMessage = gameList;
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListGames, responseMessage);
        }
    }
}
