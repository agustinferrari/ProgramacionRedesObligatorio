using Common.FileUtils;
using Common.FileUtils.Interfaces;
using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class ModifyPublishedGame : MenuStrategy
    {
        public override void HandleSelectedOption(ISocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese nombre del juego de su lista a modificar:");
            string actualGameName = Console.ReadLine();
            Console.WriteLine("Ingrese nuevo nombre:");
            string newName = Console.ReadLine();
            Console.WriteLine("Ingrese nuevo genero:");
            string genre = Console.ReadLine();
            Console.WriteLine("Ingrese nuevo sinopsis:");
            string synopsis = Console.ReadLine();
            Console.WriteLine("Ingrese el path de la caratula del juego que desea subir (.png):");
            string path = Console.ReadLine();

            string newGameData = newName + "%" + genre + "%" + synopsis;
            string gameData = actualGameName + "%" + newGameData;
            string changes = newGameData + "%" + path;
            if (actualGameName != "" && _menuHandler.ValidateAtLeastOneField(changes))
            {
                clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.ModifyPublishedGame, gameData);
                IFileHandler fileStreamHandler = new FileHandler();
                if (fileStreamHandler.FileExistsAndIsReadable(path) && fileStreamHandler.IsFilePNG(path))
                {
                    bool imageSentCorrectly = clientSocket.SendImage(path);
                    if (!imageSentCorrectly)
                        Console.WriteLine("No se pudo leer la imagen correctamente, intente modificar el juego mas tarde.");
                }
                else
                {
                    string emptyPath = "";
                    int noData = 0;
                    clientSocket.SendImageProtocolData(emptyPath, noData);
                }

                string response = clientSocket.RecieveResponse();
                Console.WriteLine(response);

                if (!(response == ResponseConstants.AuthenticationError))
                    _menuHandler.LoadLoggedUserMenu(clientSocket);
                else
                    _menuHandler.LoadMainMenu(clientSocket);
            }
            else
            {
                Console.WriteLine("Por favor ingrese el nombre del juego que quiere modificar y uno de los campos a modificar");
                _menuHandler.LoadLoggedUserMenu(clientSocket);
            }
        }
    }
}
