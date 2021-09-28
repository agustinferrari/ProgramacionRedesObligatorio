using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;


namespace ConsoleServer.Logic.Commands.Strategies
{
    public class AddGame : CommandStrategy
    {

        public override void HandleRequest(Header header, ISocketHandler clientSocketHandler)
        {
            string responseMessageResult;
            if (_clientHandler.IsSocketInUse(clientSocketHandler))
            {
                int firstElement = 0;
                int secondElement = 1;
                int thirdElement = 2;
                string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
                string[] gameData = rawData.Split('%');
                string gameName = gameData[firstElement];
                string genre = gameData[secondElement];
                string synopsis = gameData[thirdElement];
                string pathToImage = UploadImage(clientSocketHandler, gameName);

                try
                {
                    string username = _clientHandler.GetUsername(clientSocketHandler);
                    User ownerUser = _userController.GetUser(username);
                    Game newGame = new Game
                    {
                        Name = gameName,
                        Genre = genre,
                        Synopsis = synopsis,
                        Rating = 0,
                        OwnerUser = ownerUser,
                        PathToPhoto = pathToImage
                    };
                    this._gameController.AddGame(newGame);
                    responseMessageResult = ResponseConstants.AddGameSuccess;
                }
                catch (GameAlreadyAddedException)
                {
                    responseMessageResult = ResponseConstants.AddGameError;
                }
                catch (InvalidUsernameException)
                {
                    responseMessageResult = ResponseConstants.InvalidUsernameError;
                }
            }
            else
                responseMessageResult = ResponseConstants.AuthenticationError;
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.AddGame, responseMessageResult);
        }

        private string UploadImage(ISocketHandler clientSocketHandler, string gameName)
        {
            string rawImageData = clientSocketHandler.ReceiveString(SpecificationHelper.GetImageDataLength());
            ISettingsManager SettingsMgr = new SettingsManager();
            string pathToImageFolder = SettingsMgr.ReadSetting(ServerConfig.ServerPathToImageFolder);
            return clientSocketHandler.ReceiveImage(rawImageData, pathToImageFolder, gameName);
        }
    }
}
