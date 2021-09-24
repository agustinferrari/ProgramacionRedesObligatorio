using Common.NetworkUtils;
using Common.NetworkUtils.Interface;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Strategies
{
    public class GetGameDetails : MenuStrategy
    {
        public override void HandleSelectedOption(SocketHandler clientSocket)
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
                        Header receivedHeader = clientSocket.ReceiveHeader();//Capaz que sacarlo
                        string rawImageData = clientSocket.ReceiveString(SpecificationHelper.GetImageDataLength());
                        ISettingsManager SettingsMgr = new SettingsManager();
                        string pathToImageFolder = SettingsMgr.ReadSetting(ClientConfig.ClientPathToImages);
                        string pathToImageGame = clientSocket.ReceiveImage(rawImageData, pathToImageFolder);
                        Console.WriteLine("La foto fue guardada en: " + pathToImageGame);
                    }
                }
            }
            _menuHandler.LoadLoggedUserMenu(clientSocket);
        }
    }
}

