using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;


namespace ConsoleServer.Logic.Commands.Strategies
{
    public class AddGame : CommandStrategy
    {

        public override void HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            string responseMessageResult;
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
            {
                int firstElement = 0;
                int secondElement = 1;
                int thirdElement = 2;
                string rawData = clientNetworkStreamHandler.ReceiveString(header.IDataLength).Result;
                string[] gameData = rawData.Split('%');
                string gameName = gameData[firstElement];
                string genre = gameData[secondElement];
                string synopsis = gameData[thirdElement];
                string pathToImage = UploadImage(clientNetworkStreamHandler, gameName);

                try
                {
                    string username = _clientHandler.GetUsername(clientNetworkStreamHandler);
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
                    _gameController.AddGame(newGame);
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
            clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.AddGame, responseMessageResult);
        }

        private string UploadImage(INetworkStreamHandler clientNetworkStreamHandler, string gameName)
        {
            string rawImageData = clientNetworkStreamHandler.ReceiveString(SpecificationHelper.GetImageDataLength()).Result;
            ISettingsManager SettingsMgr = new SettingsManager();
            string pathToImageFolder = SettingsMgr.ReadSetting(ServerConfig.ServerPathToImageFolder);
            return clientNetworkStreamHandler.ReceiveImage(rawImageData, pathToImageFolder, gameName).Result;
        }
    }
}
