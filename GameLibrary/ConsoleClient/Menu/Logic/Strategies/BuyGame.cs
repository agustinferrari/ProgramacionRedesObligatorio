using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Strategies
{
    public class BuyGame : MenuStrategy
    {
        public override void HandleSelectedOption(SocketHandler clientSocket)
        {
            Console.WriteLine("Por favor ingrese el nombre del juego para comprar:");
            string gameName = Console.ReadLine().ToLower();
            if (_menuHandler.ValidateNotEmptyFields(gameName))
            {
                string response = _menuHandler.SendMessageAndRecieveResponse(clientSocket, CommandConstants.BuyGame, gameName);
                Console.WriteLine(response);
                bool acceptedResponses = response == ResponseConstants.BuyGameSuccess;
                acceptedResponses |= response == ResponseConstants.InvalidGameError;
                acceptedResponses |= response == ResponseConstants.GameAlreadyBought;
                if (acceptedResponses)
                    _menuHandler.LoadLoggedUserMenu(clientSocket);
                else
                    _menuHandler.LoadMainMenu(clientSocket);
            }
            else
                _menuHandler.LoadLoggedUserMenu(clientSocket);
        }
    }
}
