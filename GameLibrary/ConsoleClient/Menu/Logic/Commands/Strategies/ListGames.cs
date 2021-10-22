using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class ListGames : MenuStrategy
    {
        public override string HandleSelectedOption(ISocketHandler clientSocket)
        {
            Console.WriteLine("Desea filtrar la lista de juegos ? \n Y/N");
            string filters = Console.ReadLine().ToLower();
            string response;
            if (filters == "y" || filters == "yes")
                response = HandleListGamesFiltered(clientSocket);
            else
                response = ListGamesAvailable(clientSocket);
            return response;
        }

        public string ListGamesAvailable(ISocketHandler clientSocket)
        {
            string sendNoData = "";
            string response = clientSocket.SendMessageAndRecieveResponse(CommandConstants.ListGames, sendNoData).Result;
            return "Lista de juegos: \n" + response;
        }

        private string HandleListGamesFiltered(ISocketHandler clientSocket)
        {
            Console.WriteLine("Por favor ingrese titulo a filtrar, si no desea esta opción, ingrese enter:");
            string filterTitle = Console.ReadLine().ToLower();
            Console.WriteLine("Por favor ingrese genero a filtrar, si no desea esta opción, ingrese enter:");
            string genreFIlter = Console.ReadLine().ToLower();
            Console.WriteLine("Por favor ingrese rating minimo a filtrar, si no desea esta opción, ingrese enter:");
            string ratingTitle = Console.ReadLine().ToLower();
            string totalFilter = filterTitle + "%" + genreFIlter + "%" + ratingTitle;
            string response = clientSocket.SendMessageAndRecieveResponse(CommandConstants.ListFilteredGames, totalFilter).Result;
            return "Lista de juegos: \n" + response;
        }
    }
}
