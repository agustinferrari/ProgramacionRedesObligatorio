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
            string detailsResponse;
            string imageResponse = "";
            if (_menuValidator.ValidateNotEmptyFields(gameName))
            {
                detailsResponse = ListGameDetails(clientSocket, gameName);

                if (detailsResponse != ResponseConstants.InvalidGameError && detailsResponse != ResponseConstants.AuthenticationError)
                {
                    Console.WriteLine("Para descargar la caratula ingrese 1:");
                    string option = Console.ReadLine();
                    if (option == "1")
                    {
                        clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.GetGameImage, gameName);
                        imageResponse = clientSocket.RecieveResponse().Result;
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
            Header header = clientSocket.ReceiveHeader().Result;
            string detailsResponse = clientSocket.ReceiveString(header.IDataLength).Result;
            Console.WriteLine(detailsResponse);
            return detailsResponse;
        }

        private string ReciveAndDownloadImage(ISocketHandler clientSocket)
        {
            string rawImageData = clientSocket.ReceiveString(SpecificationHelper.GetImageDataLength()).Result;
            ISettingsManager SettingsMgr = new SettingsManager();
            string pathToImageFolder = SettingsMgr.ReadSetting(ClientConfig.ClientPathToImages);
            string pathToImageGame = clientSocket.ReceiveImage(rawImageData, pathToImageFolder, "").Result;
            string result = "";
            if (pathToImageGame != "")
                result = "La foto fue guardada en: " + pathToImageGame;
            else
                result = "El juego no tiene caratula, contactar con el administrador";
            return result;
        }

        private void ListGamesAvailable(ISocketHandler clientSocket)
        {
            ListGames listGames = new ListGames();
            Console.WriteLine(listGames.ListGamesAvailable(clientSocket));
        }


    }
}

