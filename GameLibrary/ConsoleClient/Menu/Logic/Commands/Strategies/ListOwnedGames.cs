using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class ListOwnedGames : MenuStrategy
    {
        public override string HandleSelectedOption(ISocketHandler clientSocket)
        {
            string sendNoData = "";
            string response = clientSocket.SendMessageAndRecieveResponse(CommandConstants.ListOwnedGames, sendNoData);
            return response;
        }
    }
}
