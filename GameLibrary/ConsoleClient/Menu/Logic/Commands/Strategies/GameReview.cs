using CommonProtocol.NetworkUtils;
using CommonProtocol.NetworkUtils.Interfaces;
using CommonProtocol.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class GameReview : MenuStrategy
    {
        public override async Task<string> HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            Console.WriteLine("Ingrese el nombre del juego:");
            string gameName = Console.ReadLine();
            Console.WriteLine("Ingrese el rating del juego (1-10):");
            string rating = Console.ReadLine();
            Console.WriteLine("Ingrese un comentario acerca del juego:");
            string comment = Console.ReadLine();
            string review = gameName + "%" + rating + "%" + comment;
            string response = "";
            if (_menuValidator.ValidateNotEmptyFields(gameName))
                response = await clientNetworkStream.SendMessageAndRecieveResponse(CommandConstants.ReviewGame, review);
            return response;

        }
    }
}
