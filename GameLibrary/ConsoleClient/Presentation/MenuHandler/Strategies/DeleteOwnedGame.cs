using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Presentation.MenuHandler.Strategies
{
    public class DeleteOwnedGame : MenuStrategy
    {
        public override void HandleSelectedOption(SocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese nombre del juego de su lista a modificar:");
            string gameName = Console.ReadLine();
            string response = _menuHandler.SendMessageAndRecieveResponse(clientSocket, CommandConstants.DeleteOwnedGame, gameName);
            Console.WriteLine(response);
            if (response == ResponseConstants.InvalidGameError || response == ResponseConstants.DeleteGameSuccess || response == ResponseConstants.InvalidUsernameError)
                _menuHandler.LoadLoggedUserMenu(clientSocket);
            else
                _menuHandler.LoadMainMenu(clientSocket);
        }
    }
}
