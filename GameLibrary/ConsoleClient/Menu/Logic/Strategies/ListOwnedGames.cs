using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;

namespace ConsoleClient.Menu.Logic.Strategies
{
    public class ListOwnedGames : MenuStrategy
    {
        public override void HandleSelectedOption(ISocketHandler clientSocket)
        {
            int sendNoData = 0;
            Header header = new Header(HeaderConstants.Request, CommandConstants.ListOwnedGames, sendNoData);
            clientSocket.SendHeader(header);
            string response = _menuHandler.RecieveResponse(clientSocket);
            Console.WriteLine("Lista de juegos propios:");
            Console.WriteLine(response);
            if (response == ResponseConstants.AuthenticationError)
                _menuHandler.LoadMainMenu(clientSocket);
            else
                _menuHandler.LoadLoggedUserMenu(clientSocket);
        }
    }
}
