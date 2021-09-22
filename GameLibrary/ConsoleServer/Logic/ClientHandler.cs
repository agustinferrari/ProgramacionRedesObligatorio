using Common.NetworkUtils;
using Common.Protocol;
using ConsoleServer.BussinessLogic;
using ConsoleServer.Domain;
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
        private GameController _gameController;
        private UserController _userController;
        public static bool stopHandling;

        private Dictionary<SocketHandler, string> _loggedClients;

        public ClientHandler()
        {
            _gameController = GameController.Instance;
            _userController = UserController.Instance;
            _loggedClients = new Dictionary<SocketHandler, string>();
            stopHandling = false;
        }

        public void HandleClient(SocketHandler clientSocketHandler)
        {
            bool isSocketActive = true;
            while (!stopHandling && isSocketActive)
            {
                try
                {
                    Header header = clientSocketHandler.ReceiveHeader();
                    switch (header.ICommand)
                    {
                        case CommandConstants.Login:
                            HandleLogin(header, clientSocketHandler);
                            break;
                        case CommandConstants.Logout:
                            HandleLogout(clientSocketHandler);
                            break;
                        case CommandConstants.ListGames:
                            HandleListGames(clientSocketHandler);
                            break;
                        case CommandConstants.BuyGame:
                            HandleBuyGame(header, clientSocketHandler);
                            break;
                        case CommandConstants.AddGame:
                            HandleAddGame(header, clientSocketHandler);
                            break;
                        case CommandConstants.ReviewGame:
                            HandleReviewGame(header, clientSocketHandler);
                            break;
                        case CommandConstants.GetGameDetails:
                            HandleGetGameDetails(header, clientSocketHandler);
                            break;
                        case CommandConstants.GetGameImage:
                            HandleGetGameImage(header, clientSocketHandler);
                            break;
                        case CommandConstants.ListOwnedGames:
                            HandleListOwnedGames(clientSocketHandler);
                            break;
                        case CommandConstants.ListFilteredGames:
                            HandleListFilteredGames(header, clientSocketHandler);
                            break;
                        case CommandConstants.DeleteOwnedGame:
                            HandleDeleteOwnedGame(header, clientSocketHandler);
                            break;
                        case CommandConstants.ModifyOwnedGame:
                            HandleModifyOwnedGame(header, clientSocketHandler);
                            break;
                        case 0:
                            isSocketActive = false;
                            _loggedClients.Remove(clientSocketHandler);
                            clientSocketHandler.ShutdownSocket();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Server is closing, will not process more data -> Message {e.Message}..");
                }
            }
        }

        private void HandleModifyOwnedGame(Header header, SocketHandler clientSocketHandler)
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

        private void HandleDeleteOwnedGame(Header header, SocketHandler clientSocketHandler)
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

        private void HandleListFilteredGames(Header header, SocketHandler clientSocketHandler)
        {
            string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
            string responseMessageResult = _gameController.GetGamesFiltered(rawData);

            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListFilteredGames, responseMessageResult);
        }

        private void HandleListOwnedGames(SocketHandler clientSocketHandler)
        {
            string userName = _loggedClients[clientSocketHandler];
            string gameList = _userController.ListOwnedGameByUser(userName);
            string responseMessage = gameList;
            if (gameList == "")
                responseMessage = ResponseConstants.LibraryError;
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListOwnedGames, responseMessage);
        }

        private void HandleLogin(Header header, SocketHandler clientSocketHandler)
        {
            string userName = clientSocketHandler.ReceiveString(header.IDataLength); //Podriamos hacer un metodo que haga todo esto de una
            string responseMessageResult;
            if (_loggedClients.ContainsValue(userName))
            {
                responseMessageResult = ResponseConstants.LoginErrorAlreadyLogged;
            }
            else
            {
                if (!_loggedClients.ContainsKey(clientSocketHandler))
                {
                    _loggedClients.Add(clientSocketHandler, userName);
                    _userController.TryAddUser(userName);
                    responseMessageResult = ResponseConstants.LoginSuccess;
                }
                else
                {
                    responseMessageResult = ResponseConstants.LoginErrorSocketAlreadyInUse;
                }
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.Login, responseMessageResult);
        }

        private void HandleLogout(SocketHandler clientSocketHandler)
        {
            if (_loggedClients.ContainsKey(clientSocketHandler))
                _loggedClients.Remove(clientSocketHandler);
            string responseMessageResult = ResponseConstants.LogoutSuccess;
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.Logout, responseMessageResult);
        }

        private void HandleListGames(SocketHandler clientSocketHandler)
        {
            string gameList = _gameController.GetGames();
            string responseMessage = gameList;
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListGames, responseMessage);
        }

        private void HandleBuyGame(Header header, SocketHandler clientSocketHandler)
        {
            string gameName = clientSocketHandler.ReceiveString(header.IDataLength);
            string username;
            string responseMessageResult;
            if (_loggedClients.ContainsKey(clientSocketHandler))
            {
                username = _loggedClients[clientSocketHandler];
                try
                {
                    _userController.BuyGame(username, gameName);
                    responseMessageResult = ResponseConstants.BuyGameSuccess;
                }
                catch (InvalidUsernameException e)
                {
                    responseMessageResult = ResponseConstants.InvalidUsernameError;
                }
                catch (InvalidGameException e)
                {
                    responseMessageResult = ResponseConstants.InvalidGameError;
                }
                catch (GameAlreadyBoughtException ge)
                {
                    responseMessageResult = ResponseConstants.GameAlreadyBought;
                }
            }
            else
            {
                responseMessageResult = ResponseConstants.AuthenticationError;
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.BuyGame, responseMessageResult);
        }

        private void HandleAddGame(Header header, SocketHandler clientSocketHandler)
        {
            string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
            string[] gameData = rawData.Split('%');
            string name = gameData[0];
            string genre = gameData[1];
            string synopsis = gameData[2];
            Console.WriteLine("Name: " + name + ", Genre: " + genre + ", Synopsis: " + synopsis);
            string rawImageData = clientSocketHandler.ReceiveString(SpecificationHelper.GetImageDataLength());
            string pathToImageGame = clientSocketHandler.ReceiveImage(rawImageData); //Ver donde guardarla imagen
            string responseMessageResult;
            if (_loggedClients.ContainsKey(clientSocketHandler))
            {
                string userName = _loggedClients[clientSocketHandler];
                try
                {
                    User user = _userController.GetUser(userName);
                    Game newGame = new Game
                    {
                        Name = name,
                        Genre = genre,
                        Synopsis = synopsis,
                        Rating = 0,
                        PathToPhoto = pathToImageGame,
                        userOwner = user
                    };
                    this._gameController.AddGame(newGame);
                    responseMessageResult = ResponseConstants.AddGameSuccess;
                }
                catch (InvalidUsernameException e)
                {
                    responseMessageResult = ResponseConstants.InvalidUsernameError;
                }
            }
            else
            {
                responseMessageResult = ResponseConstants.AuthenticationError;
            } 
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.AddGame, responseMessageResult);
        }

        private void HandleReviewGame(Header header, SocketHandler clientSocketHandler)
        {
            string rawData = clientSocketHandler.ReceiveString(header.IDataLength);
            string[] gameData = rawData.Split('%');
            string gameName = gameData[0];
            string rating = gameData[1];
            string comment = gameData[2];


            string responseMessageResult;
            if (_loggedClients.ContainsKey(clientSocketHandler))
            {
                string userName = _loggedClients[clientSocketHandler];
                try
                {
                    Review newReview = new Review
                    {
                        User = _userController.GetUser(userName),
                        Comment = comment,
                        Rating = Int32.Parse(rating),
                    };

                    _gameController.AddReview(gameName, newReview);
                    responseMessageResult = ResponseConstants.ReviewGameSuccess;
                }
                catch (InvalidUsernameException e)
                {
                    responseMessageResult = ResponseConstants.InvalidUsernameError;
                }
                catch (InvalidGameException e)
                {
                    responseMessageResult = ResponseConstants.InvalidGameError;
                }
                catch (Exception e) when (e is FormatException || e is InvalidReviewRatingException)
                {
                    responseMessageResult = ResponseConstants.InvalidRatingException;
                }
            }
            else
            {
                responseMessageResult = ResponseConstants.AuthenticationError;
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ReviewGame, responseMessageResult);
        }

        private void HandleGetGameDetails(Header header, SocketHandler clientSocketHandler)
        {
            string gameName = clientSocketHandler.ReceiveString(header.IDataLength);
            string responseMessageResult;
            try
            {
                Game game = _gameController.GetGame(gameName);
                responseMessageResult = game.ToString();
            }
            catch (InvalidGameException e)
            {
                responseMessageResult = ResponseConstants.InvalidGameError;
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.GetGameDetails, responseMessageResult);
        }

        private void HandleGetGameImage(Header header, SocketHandler clientSocketHandler)
        {
            string gameName = clientSocketHandler.ReceiveString(header.IDataLength);
            string responseMessageResult = "";
            Game game = null;
            try
            {
                game = _gameController.GetGame(gameName);
            }
            catch (InvalidGameException e)
            {
                responseMessageResult = ResponseConstants.InvalidGameError;
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.GetGameImage, responseMessageResult);//Capaz que hacer de otra forma
            if (game != null)
                clientSocketHandler.SendImage(game.PathToPhoto);
        }

    }
}
