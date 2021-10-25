using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class ListOwnedGames : MenuStrategy
    {
        public override string HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            string sendNoData = "";
            string response = clientNetworkStream.SendMessageAndRecieveResponse(CommandConstants.ListOwnedGames, sendNoData).Result;
            return response;
        }
    }
}
