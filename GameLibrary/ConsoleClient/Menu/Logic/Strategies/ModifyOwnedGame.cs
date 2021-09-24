using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Strategies
{
    public class ModifyOwnedGame : MenuStrategy
    {
        public override void HandleSelectedOption(SocketHandler clientSocket)
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
                if (response == ResponseConstants.InvalidGameError || response == ResponseConstants.ModifyOwnedGameSucces || response == ResponseConstants.InvalidUsernameError)
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
