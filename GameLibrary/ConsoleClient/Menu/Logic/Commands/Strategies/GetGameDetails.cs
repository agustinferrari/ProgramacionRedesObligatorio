using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;
using System.Threading.Tasks;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class GetGameDetails : MenuStrategy
    {
        public override async Task<string> HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            await ListGamesAvailable(clientNetworkStream);
            Console.WriteLine("Ingrese el nombre del juego para ver sus detalles:");
            string gameName = Console.ReadLine();
            string detailsResponse;
            string imageResponse = "";
            if (_menuValidator.ValidateNotEmptyFields(gameName))
            {
                detailsResponse = await ListGameDetails(clientNetworkStream, gameName);

                if (detailsResponse != ResponseConstants.InvalidGameError && detailsResponse != ResponseConstants.AuthenticationError)
                {
                    Console.WriteLine("Para descargar la caratula ingrese 1:");
                    string option = Console.ReadLine();
                    if (option == "1")
                    {
                        await clientNetworkStream.SendMessage(HeaderConstants.Request, CommandConstants.GetGameImage, gameName);
                        imageResponse = await clientNetworkStream.RecieveResponse();
                        if (imageResponse != ResponseConstants.InvalidGameError && imageResponse != ResponseConstants.AuthenticationError)
                            imageResponse = await ReciveAndDownloadImage(clientNetworkStream);
                    }
                    else
                        imageResponse = "La foto no fue descargada";
                }
                else
                    imageResponse = detailsResponse;
            }
            return imageResponse;
        }

        private async Task<string> ListGameDetails(INetworkStreamHandler clientNetworkStream, string gameName)
        {
            await clientNetworkStream.SendMessage(HeaderConstants.Request, CommandConstants.GetGameDetails, gameName);
            Header header = await clientNetworkStream.ReceiveHeader();
            string detailsResponse = await clientNetworkStream.ReceiveString(header.IDataLength);
            Console.WriteLine(detailsResponse);
            return detailsResponse;
        }

        private async Task<string> ReciveAndDownloadImage(INetworkStreamHandler clientNetworkStream)
        {
            string rawImageData = await clientNetworkStream.ReceiveString(SpecificationHelper.GetImageDataLength());
            ISettingsManager SettingsMgr = new SettingsManager();
            string pathToImageFolder = SettingsMgr.ReadSetting(ClientConfig.ClientPathToImages);
            string pathToImageGame = await clientNetworkStream.ReceiveImage(rawImageData, pathToImageFolder, "");
            string result = "";
            if (pathToImageGame != "")
                result = "La foto fue guardada en: " + pathToImageGame;
            else
                result = "El juego no tiene caratula, contactar con el administrador";
            return result;
        }

        private async Task ListGamesAvailable(INetworkStreamHandler clientNetworkStream)
        {
            ListGames listGames = new ListGames();
            Console.WriteLine(await listGames.ListGamesAvailable(clientNetworkStream));
        }


    }
}

