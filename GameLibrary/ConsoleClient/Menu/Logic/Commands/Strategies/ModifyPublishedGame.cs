using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class ModifyPublishedGame : MenuStrategy
    {
        public override void HandleSelectedOption(ISocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese nombre del juego de su lista a modificar:");
            string gameName = Console.ReadLine();
            Console.WriteLine("Ingrese nuevo nombre:");
            string newName = Console.ReadLine();
            Console.WriteLine("Ingrese nuevo genero:");
            string genre = Console.ReadLine();
            Console.WriteLine("Ingrese nuevo sinopsis:");
            string synopsis = Console.ReadLine();
            string changes = gameName + "%" + newName + "%" + genre + "%" + synopsis;
            if (_menuHandler.ValidateNotEmptyFields(changes))
            {
                string response = _menuHandler.SendMessageAndRecieveResponse(clientSocket, CommandConstants.ModifyPublishedGame, changes);
                Console.WriteLine(response);

                if (!(response == ResponseConstants.AuthenticationError))
                    _menuHandler.LoadLoggedUserMenu(clientSocket);
                else
                    _menuHandler.LoadMainMenu(clientSocket);
            }
            else
            {
                _menuHandler.LoadLoggedUserMenu(clientSocket);
            }
        }
    }
}
