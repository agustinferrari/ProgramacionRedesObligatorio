using Common.NetworkUtils;
using Common.NetworkUtils.Interface;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;


namespace ConsoleServer.Logic.Commands.Strategies
{
    public class AddGame : CommandStrategy
    {

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
        {
            int firstElement = 0;
            int secondElement = 1;
            int thirdElement = 2;
            string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
            string[] gameData = rawData.Split('%');
            string gameName = gameData[firstElement];
            string genre = gameData[secondElement];
            string synopsis = gameData[thirdElement];
            string rawImageData = clientSocketHandler.ReceiveString(SpecificationHelper.GetImageDataLength());
            ISettingsManager SettingsMgr = new SettingsManager();
            string pathToImageFolder = SettingsMgr.ReadSetting(ServerConfig.ServerPathToImageFolder);
            string pathToImageGame = clientSocketHandler.ReceiveImage(rawImageData, pathToImageFolder, gameName);
            string username = _clientHandler.GetUsername(clientSocketHandler);
            User ownerUser = _userController.GetUser(username);
            Game newGame = new Game
            {
                Name = gameName,
                Genre = genre,
                Synopsis = synopsis,
                Rating = 0,
                OwnerUser = ownerUser,
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
