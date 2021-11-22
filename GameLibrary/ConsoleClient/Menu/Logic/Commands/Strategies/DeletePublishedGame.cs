using CommonProtocol.NetworkUtils.Interfaces;
using CommonProtocol.Protocol;
using System;
using System.Threading.Tasks;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class DeletePublishedGame : MenuStrategy
    {
        public override async Task<string> HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            Console.WriteLine("Ingrese nombre del juego de su lista a eliminar:");
            string gameName = Console.ReadLine();
            string response = "";
            if (_menuValidator.ValidateNotEmptyFields(gameName))
                response = await clientNetworkStream.SendMessageAndRecieveResponse(CommandConstants.DeletePublishedGame, gameName);
            return response;
        }
    }
}
