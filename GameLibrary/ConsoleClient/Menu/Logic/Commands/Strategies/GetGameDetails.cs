using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;


namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class GetGameDetails : MenuStrategy
    {
        public override string HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            ListGamesAvailable(clientNetworkStream);
            Console.WriteLine("Ingrese el nombre del juego para ver sus detalles:");
            string gameName = Console.ReadLine();
            string detailsResponse;
            string imageResponse = "";
            if (_menuValidator.ValidateNotEmptyFields(gameName))
            {
                detailsResponse = ListGameDetails(clientNetworkStream, gameName);

                if (detailsResponse != ResponseConstants.InvalidGameError && detailsResponse != ResponseConstants.AuthenticationError)
                {
                    Console.WriteLine("Para descargar la caratula ingrese 1:");
                    string option = Console.ReadLine();
                    if (option == "1")
                    {
                        clientNetworkStream.SendMessage(HeaderConstants.Request, CommandConstants.GetGameImage, gameName);
                        imageResponse = clientNetworkStream.RecieveResponse().Result;
                        if (imageResponse != ResponseConstants.InvalidGameError && imageResponse != ResponseConstants.AuthenticationError)
                            imageResponse = ReciveAndDownloadImage(clientNetworkStream);
                    }
                    else
                        imageResponse = "La foto no fue descargada";
                }
                else
                    imageResponse = detailsResponse;
            }
            return imageResponse;
        }

        private string ListGameDetails(INetworkStreamHandler clientNetworkStream, string gameName)
        {
            clientNetworkStream.SendMessage(HeaderConstants.Request, CommandConstants.GetGameDetails, gameName);
            Header header = clientNetworkStream.ReceiveHeader().Result;
            string detailsResponse = clientNetworkStream.ReceiveString(header.IDataLength).Result;
            Console.WriteLine(detailsResponse);
            return detailsResponse;
        }

        private string ReciveAndDownloadImage(INetworkStreamHandler clientNetworkStream)
        {
            string rawImageData = clientNetworkStream.ReceiveString(SpecificationHelper.GetImageDataLength()).Result;
            ISettingsManager SettingsMgr = new SettingsManager();
            string pathToImageFolder = SettingsMgr.ReadSetting(ClientConfig.ClientPathToImages);
            string pathToImageGame = clientNetworkStream.ReceiveImage(rawImageData, pathToImageFolder, "").Result;
            string result = "";
            if (pathToImageGame != "")
                result = "La foto fue guardada en: " + pathToImageGame;
            else
                result = "El juego no tiene caratula, contactar con el administrador";
            return result;
        }

        private void ListGamesAvailable(INetworkStreamHandler clientNetworkStream)
        {
            ListGames listGames = new ListGames();
            Console.WriteLine(listGames.ListGamesAvailable(clientNetworkStream));
        }


    }
}

