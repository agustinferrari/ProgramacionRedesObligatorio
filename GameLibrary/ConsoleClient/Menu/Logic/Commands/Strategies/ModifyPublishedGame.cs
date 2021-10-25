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
        public override string HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
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
            string response;
            if (actualGameName != "" && ValidateAtLeastOneField(changes))
            {
                clientNetworkStream.SendMessage(HeaderConstants.Request, CommandConstants.ModifyPublishedGame, gameData);
                IFileHandler fileStreamHandler = new FileHandler();
                if (fileStreamHandler.FileExistsAndIsReadable(path) && fileStreamHandler.IsFilePNG(path))
                {
                    bool imageSentCorrectly = clientNetworkStream.SendImage(path).Result;
                    if (!imageSentCorrectly)
                        Console.WriteLine("No se pudo leer la imagen correctamente, intente modificar el juego mas tarde.");
                }
                else
                {
                    string emptyPath = "";
                    int noData = 0;
                    clientNetworkStream.SendImageProtocolData(emptyPath, noData);
                }
                response = clientNetworkStream.RecieveResponse().Result;
            }
            else
                response = "Por favor ingrese el nombre del juego que quiere modificar y uno de los campos a modificar";
            return response;
        }

        private bool ValidateAtLeastOneField(string data)
        {
            string[] separatedData = data.Split("%");
            foreach (string field in separatedData)
                if (field != "")
                {
                    return true;
                }
            return false;
        }
    }
}
