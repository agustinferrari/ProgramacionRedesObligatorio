using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Presentation.MenuHandler.Strategies
{
    public class GameReview : MenuStrategy
    {
        public override void HandleSelectedOption(SocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese el nombre del juego:");
            string gameName = Console.ReadLine();
            Console.WriteLine("Ingrese el rating del juego (1-10):");
            string rating = Console.ReadLine();
            Console.WriteLine("Ingrese un comentario acerca del juego:");
            string comment = Console.ReadLine();
            string review = gameName + "%" + rating + "%" + comment;
            if (_menuHandler.ValidateNotEmptyFields(gameName))
            {
                string response = _menuHandler.SendMessageAndRecieveResponse(clientSocket, CommandConstants.ReviewGame, review);
                Console.WriteLine(response);
                if (response == ResponseConstants.ReviewGameSuccess || response == ResponseConstants.InvalidGameError
                        || response == ResponseConstants.InvalidRatingException)
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
