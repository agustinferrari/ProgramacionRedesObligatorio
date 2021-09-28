using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;


namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class DeletePublishedGame : MenuStrategy
    {
        public override void HandleSelectedOption(ISocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese nombre del juego de su lista a eliminar:");
            string gameName = Console.ReadLine();
            string response = clientSocket.SendMessageAndRecieveResponse(CommandConstants.DeletePublishedGame, gameName);
            Console.WriteLine(response);
            if(!(response == ResponseConstants.AuthenticationError))
                _menuHandler.LoadLoggedUserMenu(clientSocket);
            else
                _menuHandler.LoadMainMenu(clientSocket);
        }
    }
}
