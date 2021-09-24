using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Strategies
{
    public class AddGame : MenuStrategy
    {
        public override void HandleSelectedOption(SocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese el nombre del juego:");
            string name = Console.ReadLine();
            Console.WriteLine("Ingrese el genero del juego:");
            string genre = Console.ReadLine();
            Console.WriteLine("Ingrese un resumen del juego:");
            string synopsis = Console.ReadLine();
            Console.WriteLine("Ingrese el path de la caratula del juego que desea subir:");
            string path = Console.ReadLine();

            string gameData = name + "%" + genre + "%" + synopsis;
            string dataToCheck = gameData + "%" + path;
            if (_menuHandler.ValidateNotEmptyFields(dataToCheck))
            {
                clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.AddGame, gameData);
                clientSocket.SendImage(path);

                Header recivedHeader = clientSocket.ReceiveHeader();
                string response = clientSocket.ReceiveString(recivedHeader.IDataLength);
                Console.WriteLine(response);
                if (response == ResponseConstants.AddGameSuccess)
                    _menuHandler.LoadLoggedUserMenu(clientSocket);
                else
                    _menuHandler.LoadMainMenu(clientSocket);
            }
            else
                _menuHandler.LoadLoggedUserMenu(clientSocket);
        }
    }
}
