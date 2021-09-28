using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;


namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class DeletePublishedGame : MenuStrategy
    {
        public override string HandleSelectedOption(ISocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese nombre del juego de su lista a eliminar:");
            string gameName = Console.ReadLine();
            string response = "";
            if (_menuHandler.ValidateNotEmptyFields(gameName))
                response = clientSocket.SendMessageAndRecieveResponse(CommandConstants.DeletePublishedGame, gameName);
            return response;
        }
    }
}
