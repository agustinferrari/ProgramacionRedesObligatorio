using CommonProtocol.NetworkUtils.Interfaces;
using CommonProtocol.Protocol;
using System;
using System.Threading.Tasks;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class BuyGame : MenuStrategy
    {
        public override async Task<string> HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            await ListGamesAvailable(clientNetworkStream);
            Console.WriteLine("Por favor ingrese el nombre del juego para comprar:");
            string gameName = Console.ReadLine().ToLower();
            string response = "";
            if (_menuValidator.ValidateNotEmptyFields(gameName))
                response = await clientNetworkStream.SendMessageAndRecieveResponse(CommandConstants.BuyGame, gameName);
            return response;
        }

        private async Task ListGamesAvailable(INetworkStreamHandler clientNetworkStream)
        {
            ListGames listGames = new ListGames();
            Console.WriteLine(await listGames.ListGamesAvailable(clientNetworkStream));
        }
    }
}
