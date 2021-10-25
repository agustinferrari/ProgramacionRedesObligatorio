using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;


namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class DeletePublishedGame : MenuStrategy
    {
        public override string HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            Console.WriteLine("Ingrese nombre del juego de su lista a eliminar:");
            string gameName = Console.ReadLine();
            string response = "";
            if (_menuValidator.ValidateNotEmptyFields(gameName))
                response = clientNetworkStream.SendMessageAndRecieveResponse(CommandConstants.DeletePublishedGame, gameName).Result;
            return response;
        }
    }
}
