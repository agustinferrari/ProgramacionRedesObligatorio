using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Presentation.MenuHandler.Strategies
{
    public class ListGamesMainMenu : MenuStrategy
    {
        public override void HandleSelectedOption(SocketHandler clientSocket)
        {
            Console.WriteLine("Desea filtrar la lista de juegos ? \n Y/N");
            string filterResponse = Console.ReadLine().ToLower();
            if (filterResponse == "y" || filterResponse == "yes")
                _menuHandler.HandleListGamesFiltered(clientSocket);
            else
            {
                Header header = new Header(HeaderConstants.Request, CommandConstants.ListGames, 0);
                clientSocket.SendHeader(header);
                Header recivedHeader = clientSocket.ReceiveHeader();
                string response = clientSocket.ReceiveString(recivedHeader.IDataLength);
                Console.WriteLine("Lista de juegos:");
                Console.WriteLine(response);
            }
            _menuHandler.LoadMainMenu(clientSocket);
        }

    }
}
