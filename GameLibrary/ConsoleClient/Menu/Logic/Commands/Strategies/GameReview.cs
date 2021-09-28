using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class GameReview : MenuStrategy
    {
        public override string HandleSelectedOption(ISocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese el nombre del juego:");
            string gameName = Console.ReadLine();
            Console.WriteLine("Ingrese el rating del juego (1-10):");
            string rating = Console.ReadLine();
            Console.WriteLine("Ingrese un comentario acerca del juego:");
            string comment = Console.ReadLine();
            string review = gameName + "%" + rating + "%" + comment;
            string response = "";
            if (_menuHandler.ValidateNotEmptyFields(gameName))
                response = clientSocket.SendMessageAndRecieveResponse(CommandConstants.ReviewGame, review);
            return response;

        }
    }
}
