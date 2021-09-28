using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;


namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class GetGameDetails : MenuStrategy
    {
        public override string HandleSelectedOption(ISocketHandler clientSocket)
        {
            ListGamesAvailable(clientSocket);
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
                        if (imageResponse != ResponseConstants.InvalidGameError && imageResponse != ResponseConstants.AuthenticationError)
                            imageResponse = ReciveAndDownloadImage(clientSocket);
                    }
                    else
                        imageResponse = "La foto no fue descargada";
                }
                else
                    imageResponse = detailsResponse;
            }
            return imageResponse;
        }

        private string ListGameDetails(ISocketHandler clientSocket, string gameName)
        {
            clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.GetGameDetails, gameName);
            Header header = clientSocket.ReceiveHeader();
            string detailsResponse = clientSocket.ReceiveString(header.IDataLength);
            return detailsResponse;
        }

        private string ReciveAndDownloadImage(ISocketHandler clientSocket)
        {
            string rawImageData = clientSocket.ReceiveString(SpecificationHelper.GetImageDataLength());
            ISettingsManager SettingsMgr = new SettingsManager();
            string pathToImageFolder = SettingsMgr.ReadSetting(ClientConfig.ClientPathToImages);
            string pathToImageGame = clientSocket.ReceiveImage(rawImageData, pathToImageFolder, "");
            return "La foto fue guardada en: " + pathToImageGame;
        }

        private void ListGamesAvailable(ISocketHandler clientSocket)
        {
            ListGames listGames = new ListGames();
            listGames.ListGamesAvailable(clientSocket);
        }


    }
}

