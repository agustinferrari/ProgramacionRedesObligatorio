using CommonProtocol.NetworkUtils.Interfaces;
using CommonProtocol.Protocol;
using System;
using System.Threading.Tasks;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class ListOwnedGames : MenuStrategy
    {
        public override async Task<string> HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            string sendNoData = "";
            string response = await clientNetworkStream.SendMessageAndRecieveResponse(CommandConstants.ListOwnedGames, sendNoData);
            return response;
        }
    }
}
