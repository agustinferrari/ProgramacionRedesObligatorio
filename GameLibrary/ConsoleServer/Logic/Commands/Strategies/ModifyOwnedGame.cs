using Common.NetworkUtils;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class ModifyOwnedGame : CommandStrategy
    {

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
        {
            string responseMessage;
            if (_clientHandler.IsSocketInUse(clientSocketHandler))
            {
                string userName = _clientHandler.GetUsername(clientSocketHandler);
                string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
                string[] gameData = rawData.Split('%');
                string oldGameName = gameData[0];
                string newGameName = gameData[1];
                string newGamegenre = gameData[2];
                string newGameSynopsis = gameData[3];
                Game newGame = new Game
                {
                    Name = newGameName,
                    Genre = newGamegenre,
                    Synopsis = newGameSynopsis
                };
                try
                {
                    _userController.ModifyGame(userName, oldGameName, newGame);
                    responseMessage = ResponseConstants.ModifyOwnedGameSucces;
                }
                catch (InvalidUsernameException e)
                {
                    responseMessage = ResponseConstants.InvalidUsernameError;
                }
                catch (GameDoesNotExistOnLibraryExcpetion e)
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
