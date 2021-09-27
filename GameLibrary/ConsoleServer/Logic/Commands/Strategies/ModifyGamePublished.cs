using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class ModifyGamePublished : CommandStrategy
    {

        public override void HandleRequest(Header header, ISocketHandler clientSocketHandler)
        {
            string responseMessage;
            if (_clientHandler.IsSocketInUse(clientSocketHandler))
            {
                int firstElement = 0;
                int secondElement = 1;
                int thirdElement = 2;
                int fouthElement = 3;
                string userName = _clientHandler.GetUsername(clientSocketHandler);
                string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
                string[] gameData = rawData.Split('%');
                string oldGameName = gameData[firstElement];
                string newGameName = gameData[secondElement];
                string newGamegenre = gameData[thirdElement];
                string newGameSynopsis = gameData[fouthElement];
                User user = _userController.GetUser(userName);
                Game newGame = new Game
                {
                    Name = newGameName,
                    Genre = newGamegenre,
                    Synopsis = newGameSynopsis,
                    userOwner = user
                };
                try
                {
                    Game gameToModify = _gameController.GetCertainGamePublishedByUser(user, oldGameName);
                    if (gameToModify != null)
                    {
                        _userController.ModifyGameFromAllUser(gameToModify, newGame);
                        _gameController.ModifyGame(gameToModify, newGame);
                        responseMessage = ResponseConstants.ModifyPublishedGameSuccess;
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
                catch (GameDoesNotExistOnLibraryExcpetion)
                {
                    responseMessage = ResponseConstants.InvalidGameError;
                }
            }
            else
                responseMessage = ResponseConstants.AuthenticationError;
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListOwnedGames, responseMessage);  
        }
    }
}
