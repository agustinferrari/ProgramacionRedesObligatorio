using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Strategies
{
    public class ListGamesMainMenu : MenuStrategy
    {
        public override void HandleSelectedOption(ISocketHandler clientSocket)
        {
            Console.WriteLine("Desea filtrar la lista de juegos ? \n Y/N");
            string filterResponse = Console.ReadLine().ToLower();
            if (filterResponse == "y" || filterResponse == "yes")
                _menuHandler.HandleListGamesFiltered(clientSocket);
            else
            {
                int sendNoData = 0;
                Header header = new Header(HeaderConstants.Request, CommandConstants.ListGames, sendNoData);
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
