﻿using Common.FileUtils;
using Common.FileUtils.Interfaces;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class AddGame : MenuStrategy
    {
        public override void HandleSelectedOption(ISocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese el nombre del juego:");
            string name = Console.ReadLine();
            Console.WriteLine("Ingrese el genero del juego:");
            string genre = Console.ReadLine();
            Console.WriteLine("Ingrese un resumen del juego:");
            string synopsis = Console.ReadLine();
            Console.WriteLine("Ingrese el path de la caratula del juego que desea subir (.png):");
            string path = Console.ReadLine();

            string gameData = name + "%" + genre + "%" + synopsis;
            string dataToCheck = gameData + "%" + path;
            if (_menuHandler.ValidateNotEmptyFields(dataToCheck))
            {
                IFileHandler fileStreamHandler = new FileHandler();
                if (fileStreamHandler.FileExistsAndIsReadable(path) && fileStreamHandler.IsFilePNG(path))
                {
                    clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.AddGame, gameData);
                    bool imageSentCorrectly = clientSocket.SendImage(path);
                    if (!imageSentCorrectly)
                        Console.WriteLine("No se pudo leer la imagen correctamente, intente modificar el juego mas tarde.");

                    string response = clientSocket.RecieveResponse();
                    Console.WriteLine(response);
                    if (response == ResponseConstants.AddGameSuccess || response == ResponseConstants.AddGameError)
                        _menuHandler.LoadLoggedUserMenu(clientSocket);
                    else
                        _menuHandler.LoadMainMenu(clientSocket);
                }
                else
                {
                    Console.WriteLine("El path ingresado es invalido o no tiene permisos para leer la imagen, intente de nuevo \n(recuerde que debe ser de tipo png)");
                    _menuHandler.LoadLoggedUserMenu(clientSocket);
                }
            }
            else
                _menuHandler.LoadLoggedUserMenu(clientSocket);
        }
    }
}