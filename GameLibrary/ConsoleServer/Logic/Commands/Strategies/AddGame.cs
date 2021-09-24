using Common.NetworkUtils;
using Common.NetworkUtils.Interface;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;
using System;


namespace ConsoleServer.Logic.Commands.Strategies
{
    public class AddGame : CommandStrategy
    {

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
        {
            string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
            string[] gameData = rawData.Split('%');
            string name = gameData[0];
            string genre = gameData[1];
            string synopsis = gameData[2];
            Console.WriteLine("Name: " + name + ", Genre: " + genre + ", Synopsis: " + synopsis);
            string rawImageData = clientSocketHandler.ReceiveString(SpecificationHelper.GetImageDataLength());
            ISettingsManager SettingsMgr = new SettingsManager();
            string pathToImageFolder = SettingsMgr.ReadSetting(ServerConfig.ServerPathToImageFolder);
            string pathToImageGame = clientSocketHandler.ReceiveImage(rawImageData, pathToImageFolder);
            Game newGame = new Game
            {
                Name = name,
                Genre = genre,
                Synopsis = synopsis,
                Rating = 0,
                PathToPhoto = pathToImageGame
            };
            string responseMessageResult;
            try
            {
                this._gameController.AddGame(newGame);
                responseMessageResult = ResponseConstants.AddGameSuccess;
            }
            catch (GameAlreadyAddedException)
            {
                responseMessageResult = ResponseConstants.AddGameError;
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.AddGame, responseMessageResult);
        }
    }
}
