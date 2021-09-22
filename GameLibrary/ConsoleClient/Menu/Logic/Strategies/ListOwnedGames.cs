using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Strategies
{
    public class ListOwnedGames : MenuStrategy
    {
        public override void HandleSelectedOption(SocketHandler clientSocket)
        {
            Header header = new Header(HeaderConstants.Request, CommandConstants.ListOwnedGames, 0);
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
