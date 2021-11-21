using Common.FileUtils;
using Common.FileUtils.Interfaces;
using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System.Threading.Tasks;
using LogsModels;
using Server.Domain;
using Server.Utils.CustomExceptions;

namespace Server.Logic.Commands.Strategies
{
    public class ModifyGamePublished : CommandStrategy
    {

        public override async Task<LogGameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            LogGameModel log = new LogGameModel(header.ICommand);
            string responseMessage;
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
            {
                int firstElement = 0;
                int secondElement = 1;
                int thirdElement = 2;
                int fouthElement = 3;
                string userName = _clientHandler.GetUsername(clientNetworkStreamHandler);
                string rawData = await clientNetworkStreamHandler.ReceiveString(header.IDataLength);
                string[] gameData = rawData.Split('%');
                string oldGameName = gameData[firstElement];
                string newGameName = gameData[secondElement];
                string newGamegenre = gameData[thirdElement];
                string newGameSynopsis = gameData[fouthElement];
                string gameName = (newGameName == "") ? oldGameName : newGameName;
                string pathToImage = await UpdateImage(clientNetworkStreamHandler, gameName);
                log.User = userName;
                log.Game = gameName;

                User user = _userController.GetUser(userName);
                Game newGame = new Game
                {
                    Name = newGameName,
                    Genre = newGamegenre,
                    Synopsis = newGameSynopsis,
                    OwnerUser = user,
                    PathToPhoto = pathToImage
                };
                responseMessage = ModifyGame(newGame, user, oldGameName, log);
            }
            else
                responseMessage = ResponseConstants.AuthenticationError;
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListOwnedGames, responseMessage);
            return log;
        }

        private string ModifyGame(Game newGame, User user, string oldGameName, LogGameModel log)
        {
            string responseMessage;
            try
            {
                Game gameToModify = _gameController.GetCertainGamePublishedByUser(user, oldGameName);
                if (gameToModify != null)
                {
                    DeletePreviousImage(gameToModify, newGame);
                    _gameController.ModifyGame(gameToModify, newGame);
                    responseMessage = ResponseConstants.ModifyPublishedGameSuccess;
                    log.Result = true;
                }
                else
                {
                    responseMessage = ResponseConstants.UnauthorizedGame;
                }
            }
            catch (InvalidUsernameException)
            {
                responseMessage = ResponseConstants.InvalidUsernameError;
            }
            catch (InvalidGameException)
            {
                responseMessage = ResponseConstants.InvalidGameError;
            }
            return responseMessage;
        }

        private async Task<string> UpdateImage(INetworkStreamHandler clientNetworkStreamHandler, string gameName)
        {
            int imageDataLength = SpecificationHelper.GetImageDataLength();
            string rawImageData = await clientNetworkStreamHandler.ReceiveString(imageDataLength);
            string emptyImageData = 0.ToString("D" + imageDataLength);
            string pathToImageGame = "";

            if (rawImageData != emptyImageData)
            {
                ISettingsManager SettingsMgr = new SettingsManager();
                string pathToImageFolder = SettingsMgr.ReadSetting(ServerConfig.ServerPathToImageFolder);
                pathToImageGame = await clientNetworkStreamHandler.ReceiveImage(rawImageData, pathToImageFolder, gameName);
            }
            return pathToImageGame;
        }

        private void DeletePreviousImage(Game gameToModify, Game newGame)
        {
            IFileHandler fileStreamHandler = new FileHandler();
            string previousImagePath = gameToModify.PathToPhoto;
            if (newGame.PathToPhoto != "")
                fileStreamHandler.DeleteFile(previousImagePath);
        }
    }
}
