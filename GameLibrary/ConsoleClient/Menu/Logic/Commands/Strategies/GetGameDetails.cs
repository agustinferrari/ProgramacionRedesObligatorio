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
            string detailsResponse = "";
            string imageResponse = "";
            if (_menuHandler.ValidateNotEmptyFields(gameName))
            {
                detailsResponse = ListGameDetails(clientSocket, gameName);

                if (detailsResponse != ResponseConstants.InvalidGameError && detailsResponse != ResponseConstants.AuthenticationError)
                {
                    Console.WriteLine("Para descargar la caratula ingrese 1:");
                    string option = Console.ReadLine();
                    if (option == "1")
                    {
                        clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.GetGameImage, gameName);
                        imageResponse = clientSocket.RecieveResponse();
                        if (imageResponse != ResponseConstants.InvalidGameError && detailsResponse != ResponseConstants.AuthenticationError)
                        {
                            ReciveAndDownloadImage(clientSocket);
                        }
                        else
                            Console.WriteLine(imageResponse);
                    }
                    else
                        Console.WriteLine("La foto no fue descargada");
                }
            }
            if (detailsResponse == ResponseConstants.AuthenticationError || imageResponse == ResponseConstants.AuthenticationError)
                _menuHandler.LoadMainMenu(clientSocket);
            else
                _menuHandler.LoadLoggedUserMenu(clientSocket);
        }

        private string ListGameDetails(ISocketHandler clientSocket, string gameName)
        {
            clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.GetGameDetails, gameName);
            Header header = clientSocket.ReceiveHeader();
            string detailsResponse = clientSocket.ReceiveString(header.IDataLength);
            Console.WriteLine(detailsResponse);
            return detailsResponse;
        }

        private void ReciveAndDownloadImage(ISocketHandler clientSocket)
        {
            string rawImageData = clientSocket.ReceiveString(SpecificationHelper.GetImageDataLength());
            ISettingsManager SettingsMgr = new SettingsManager();
            string pathToImageFolder = SettingsMgr.ReadSetting(ClientConfig.ClientPathToImages);
            string pathToImageGame = clientSocket.ReceiveImage(rawImageData, pathToImageFolder, "");
            Console.WriteLine("La foto fue guardada en: " + pathToImageGame);
        }
    }
}

