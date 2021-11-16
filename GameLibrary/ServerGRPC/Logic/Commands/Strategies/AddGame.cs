using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using CommonLog;
using ServerGRPC.Domain;
using ServerGRPC.Utils.CustomExceptions;
using System.Threading.Tasks;

namespace ServerGRPC.Logic.Commands.Strategies
{
    public class AddGame : CommandStrategy
    {

        public override async Task<GameLogModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            GameLogModel log = new GameLogModel(header.ICommand);
            string responseMessageResult;
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
            {
                int firstElement = 0;
                int secondElement = 1;
                int thirdElement = 2;
                string rawData = await clientNetworkStreamHandler.ReceiveString(header.IDataLength);
                string[] gameData = rawData.Split('%');
                string gameName = gameData[firstElement];
                string genre = gameData[secondElement];
                string synopsis = gameData[thirdElement];
                string pathToImage = await UploadImage(clientNetworkStreamHandler, gameName);
                log.Game = gameName;

                try
                {
                    string username = _clientHandler.GetUsername(clientNetworkStreamHandler);
                    log.User = username;
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
                    log.Result = true;
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
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.AddGame, responseMessageResult);
            return log;
        }

        private async Task<string> UploadImage(INetworkStreamHandler clientNetworkStreamHandler, string gameName)
        {
            string rawImageData = await clientNetworkStreamHandler.ReceiveString(SpecificationHelper.GetImageDataLength());
            ISettingsManager SettingsMgr = new SettingsManager();
            string pathToImageFolder = SettingsMgr.ReadSetting(ServerConfig.ServerPathToImageFolder);
            return await clientNetworkStreamHandler.ReceiveImage(rawImageData, pathToImageFolder, gameName);
        }
    }
}
