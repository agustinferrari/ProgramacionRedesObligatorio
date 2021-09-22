using Common.NetworkUtils;
using Common.Protocol;
using ConsoleServer.BussinessLogic;
using ConsoleServer.Domain;
using ConsoleServer.Logic.Commands.Factory;
using ConsoleServer.Logic.Commands.Strategies;
using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConsoleServer.Logic
{
    public class ClientHandler
    {
        public static bool stopHandling;

        private Dictionary<SocketHandler, string> _loggedClients;
        private static readonly object _padlock = new object();
        private static ClientHandler _instance;

        public ClientHandler()
        {
            _loggedClients = new Dictionary<SocketHandler, string>();
            stopHandling = false;
        }

        public static ClientHandler Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ClientHandler();
                    }
                    return _instance;
                }
            }
        }

        public bool IsClientLogged(string userName)
        {
            lock (_padlock)
                return _loggedClients.ContainsValue(userName);
        }

        public void HandleModify()
        {

            string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
            string[] gameData = rawData.Split('%');
            string oldGameName = gameData[0];
            string newGameName = gameData[1];
            string newGamegenre = gameData[2];
            string newGameSynopsis = gameData[3];
            string responseMessage;
            if (_loggedClients.ContainsKey(clientSocketHandler))
            {
                string userName = _loggedClients[clientSocketHandler];
                try
                {
                    User user = _userController.GetUser(userName);
                    Game newGame = new Game
                    {
                        Name = newGameName,
                        Genre = newGamegenre,
                        Synopsis = newGameSynopsis,
                        userOwner = user
                    };
                    Game gameToModify = _gameController.GetCertainGamePublishedByUser(user, oldGameName);
                    if (gameToModify != null)
                    {
                        _userController.ModifyGameFromAllUser(gameToModify, newGame);
                        _gameController.ModifyGame(gameToModify, newGame);
                        responseMessage = ResponseConstants.ModifyOwnedGameSucces;
                    }
                    else
                    {
                        responseMessage = ResponseConstants.UnauthorizedGame;
                    }
                }
                catch (InvalidUsernameException e)
                {
                    responseMessage = ResponseConstants.InvalidUsernameError;
                }
                catch (InvalidGameException e)
                {
                    responseMessage = ResponseConstants.InvalidGameError;
                }
            }
            else
            {
                responseMessage = ResponseConstants.AuthenticationError;
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListOwnedGames, responseMessage);
        }

        public bool IsSocketInUse(SocketHandler socketHandler)
        {
            lock (_padlock)
                return _loggedClients.ContainsKey(socketHandler);
        }
        public void HandleDeleteGame (SocketHandler clientSocketHandler)
        {

            string gameName = clientSocketHandler.ReceiveString(header.IDataLength);
            string responseMessage;
            if (_loggedClients.ContainsKey(clientSocketHandler))
            {
                string userName = _loggedClients[clientSocketHandler];
                try
                {
                    User user = _userController.GetUser(userName);
                    Game gameToDelete = _gameController.GetCertainGamePublishedByUser(user, gameName);
                    if(gameToDelete != null)
                    {
                    _userController.DeleteGameFromAllUsers(gameToDelete);
                    _gameController.DeletePublishedGameByUser(gameToDelete);
                    responseMessage = ResponseConstants.DeleteGameSucces;
                    }
                    else
                        responseMessage = ResponseConstants.UnauthorizedGame;
                }
                catch (InvalidUsernameException e)
                {
                    responseMessage = ResponseConstants.InvalidUsernameError;
                }
                catch (InvalidGameException e)
                {
                    responseMessage = ResponseConstants.InvalidGameError;
                }
            }
            else
            {
                responseMessage = ResponseConstants.AuthenticationError;
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListGames, responseMessage);
        }

        public string GetUsername(SocketHandler socketHandler)
        {
            lock (_padlock)
                return _loggedClients[socketHandler];
        }

        public void AddClient(SocketHandler socketHandler, string userName)
        {
            lock (_padlock)
                _loggedClients.Add(socketHandler, userName);
        }

        public void RemoveClient(SocketHandler socketHandler)
        {
            lock (_padlock)
                _loggedClients.Remove(socketHandler);
        }

        public void HandleClient(SocketHandler clientSocketHandler)
        {
            bool isSocketActive = true;
            while (!stopHandling && isSocketActive)
            {
                try
                {
                    Header header = clientSocketHandler.ReceiveHeader();
                    if (header.ICommand == 0) // Cuando el cliente cierra la consola, se envia en header con command en 0
                    {
                        isSocketActive = false;
                        _loggedClients.Remove(clientSocketHandler);
                        clientSocketHandler.ShutdownSocket();
                    }
                    else
                    {
                        CommandStrategy commandStrategy = CommandFactory.GetStrategy(header.ICommand);
                        commandStrategy.HandleRequest(header, clientSocketHandler);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Server is closing, will not process more data -> Message {e.Message}..");
                }
            }
        }
    }
}
