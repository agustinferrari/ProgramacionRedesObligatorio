using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class ListOwnedGames : MenuStrategy
    {
        public override string HandleSelectedOption(ISocketHandler clientSocket)
        {
            string response = ListOwnedGamesByUser(clientSocket);
            return response;
            /*if (response == ResponseConstants.AuthenticationError)
                _menuHandler.LoadMainMenu(clientSocket);
            else
                _menuHandler.LoadLoggedUserMenu(clientSocket);*/
        }

        public string ListOwnedGamesByUser(ISocketHandler clientSocket)
        {
            string sendNoData = "";
            /*Header header = new Header(HeaderConstants.Request, CommandConstants.ListOwnedGames, sendNoData);
            clientSocket.SendHeader(header);*/
            string response = clientSocket.SendMessageAndRecieveResponse(CommandConstants.ListOwnedGames, sendNoData);
            return response;
        }
    }
}
