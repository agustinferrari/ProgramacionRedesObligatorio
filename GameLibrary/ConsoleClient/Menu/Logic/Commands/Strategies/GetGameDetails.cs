using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class GetGameDetails : MenuStrategy
    {
        public override void HandleSelectedOption(ISocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese el nombre del juego para ver sus detalles:");
            string gameName = Console.ReadLine();
            if (_menuHandler.ValidateNotEmptyFields(gameName))
            {
                clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.GetGameDetails, gameName);
                Header header = clientSocket.ReceiveHeader();
                string response = clientSocket.ReceiveString(header.IDataLength);
                Console.WriteLine(response);
                if (response != ResponseConstants.InvalidGameError)
                {
                    Console.WriteLine("Para descargar la caratula ingrese 1:");
                    string option = Console.ReadLine();
                    if (option == "1")
                    {
                        clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.GetGameImage, gameName);
                        string imageResponse = clientSocket.RecieveResponse();
                        if (imageResponse != ResponseConstants.InvalidGameError)
                        {
                            string rawImageData = clientSocket.ReceiveString(SpecificationHelper.GetImageDataLength());
                            ISettingsManager SettingsMgr = new SettingsManager();
                            string pathToImageFolder = SettingsMgr.ReadSetting(ClientConfig.ClientPathToImages);
                            string pathToImageGame = clientSocket.ReceiveImage(rawImageData, pathToImageFolder, "");
                            Console.WriteLine("La foto fue guardada en: " + pathToImageGame);
                        }
                        else
                            Console.WriteLine(imageResponse);
                    }
                    else
                        Console.WriteLine("La foto no fue descargada");
                }
            }
            _menuHandler.LoadLoggedUserMenu(clientSocket);
        }
    }
}

