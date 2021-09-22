using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class ListFilteredGames : CommandStrategy
    {

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
        {
            string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
            string responseMessageResult = _gameController.GetGamesFiltered(rawData);

            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListFilteredGames, responseMessageResult);
        }
    }
}
