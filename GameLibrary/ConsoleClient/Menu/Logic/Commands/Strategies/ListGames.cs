using CommonProtocol.NetworkUtils;
using CommonProtocol.NetworkUtils.Interfaces;
using CommonProtocol.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class ListGames : MenuStrategy
    {
        public override async Task<string> HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            Console.WriteLine("Desea filtrar la lista de juegos ? \n Y/N");
            string filters = Console.ReadLine().ToLower();
            string response;
            if (filters == "y" || filters == "yes")
                response = await HandleListGamesFiltered(clientNetworkStream);
            else
                response = await ListGamesAvailable(clientNetworkStream);
            return response;
        }

        public async Task<string> ListGamesAvailable(INetworkStreamHandler clientNetworkStream)
        {
            string sendNoData = "";
            string response = await clientNetworkStream.SendMessageAndRecieveResponse(CommandConstants.ListGames, sendNoData);
            return "Lista de juegos: \n" + response;
        }

        private async Task<string> HandleListGamesFiltered(INetworkStreamHandler clientNetworkStream)
        {
            Console.WriteLine("Por favor ingrese titulo a filtrar, si no desea esta opción, ingrese enter:");
            string filterTitle = Console.ReadLine().ToLower();
            Console.WriteLine("Por favor ingrese genero a filtrar, si no desea esta opción, ingrese enter:");
            string genreFIlter = Console.ReadLine().ToLower();
            Console.WriteLine("Por favor ingrese rating minimo a filtrar, si no desea esta opción, ingrese enter:");
            string ratingTitle = Console.ReadLine().ToLower();
            string totalFilter = filterTitle + "%" + genreFIlter + "%" + ratingTitle;
            string response = await clientNetworkStream.SendMessageAndRecieveResponse(CommandConstants.ListFilteredGames, totalFilter);
            return "Lista de juegos: \n" + response;
        }
    }
}
