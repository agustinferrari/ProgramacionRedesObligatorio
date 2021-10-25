using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class BuyGame : MenuStrategy
    {
        public override string HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            ListGamesAvailable(clientNetworkStream);
            Console.WriteLine("Por favor ingrese el nombre del juego para comprar:");
            string gameName = Console.ReadLine().ToLower();
            string response = "";
            if (_menuValidator.ValidateNotEmptyFields(gameName))
                response = clientNetworkStream.SendMessageAndRecieveResponse(CommandConstants.BuyGame, gameName).Result;
            return response;
        }

        private void ListGamesAvailable(INetworkStreamHandler clientNetworkStream)
        {
            ListGames listGames = new ListGames();
            Console.WriteLine(listGames.ListGamesAvailable(clientNetworkStream));
        }
    }
}
