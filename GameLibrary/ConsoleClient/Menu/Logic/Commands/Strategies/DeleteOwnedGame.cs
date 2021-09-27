using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class DeleteOwnedGame : MenuStrategy
    {
        public override void HandleSelectedOption(ISocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese nombre del juego de su lista a modificar:");
            string gameName = Console.ReadLine();
            string response = _menuHandler.SendMessageAndRecieveResponse(clientSocket, CommandConstants.DeletePublishedGame, gameName);
            Console.WriteLine(response);
            bool acceptedResponses = response == ResponseConstants.DeleteGameSuccess;
            acceptedResponses |= response == ResponseConstants.InvalidGameError;
            acceptedResponses |= response == ResponseConstants.InvalidUsernameError;
            if (acceptedResponses)
                _menuHandler.LoadLoggedUserMenu(clientSocket);
            else
                _menuHandler.LoadMainMenu(clientSocket);
        }
    }
}
