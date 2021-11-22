using CommonProtocol.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using Server.BusinessLogic.Interfaces;
using Server.Domain;
using Server.Utils.CustomExceptions;

namespace Server.BusinessLogic
{
    public class GameController : IGameController
    {
        private static readonly object _padlock = new object();
        private static GameController _instance = null;
        private List<Game> _games;

        private GameController()
        {
            _games = new List<Game>();
        }

        public static GameController Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameController();
                    }
                    return _instance;
                }
            }
        }

        public void AddGame(Game gameToAdd)
        {
            lock (_padlock)
            {
                if (_games != null && _games.Exists(game => game.Name.ToLower() == gameToAdd.Name.ToLower()))
                    throw new GameAlreadyAddedException();
                else
                {
                    _games.Add(gameToAdd);
                }
            }
        }

        public string GetAllGames()
        {
            lock (_padlock)
                if (_games != null)
                    return GameListToString(_games);
            throw new InvalidGameException();
        }

        public Game GetGame(string gameName)
        {
            lock (_padlock)
            {
                if (_games != null && _games.Exists(game => game.Name.ToLower() == gameName.ToLower()))
                    return _games.Find(game => game.Name.ToLower() == gameName.ToLower());
                throw new InvalidGameException();
            }
        }

        public void AddReview(string gameName, Review newReview)
        {
            Game gameToReview = GetGame(gameName);
            gameToReview.AddReview(newReview);
        }

        public string GetGamesFiltered(string rawData)
        {
            string emptyString = "";
            int firstElement = 0;
            int secondElement = 1;
            int thirdElement = 2;
            string[] gamesFilters = rawData.Split('%');
            string gameName = gamesFilters[firstElement].ToLower();
            string genre = gamesFilters[secondElement].ToLower();
            int rating = 0;
            if (gamesFilters[thirdElement] != emptyString)
                rating = Int32.Parse(gamesFilters[2]);

            lock (_padlock)
                if (_games != null)
                {
                    List<Game> filteredGames = _games.FindAll(game => game.Name.ToLower().Contains(gameName) && game.Genre.ToLower().Contains(genre)
                                                           && game.Rating >= rating);
                    string filteredGamesResult = GameListToString(filteredGames);
                    return filteredGamesResult;
                }
                else
                {
                    throw new InvalidGameException();
                }

        }

        private string GameListToString(List<Game> gamesToString)
        {
            string result = "";
            for (int i = 0; i < gamesToString.Count; i++)
            {
                Game game = gamesToString[i];
                result += game.Name;
                if (i < gamesToString.Count - 1)
                    result += "\n";
            }
            return result;
        }

        public Game GetCertainGamePublishedByUser(User user, string gameName)
        {
            lock (_padlock)
            {
                if (_games == null)
                    return null;
                Game userGame = _games.Find(game => game.Name.ToLower() == gameName.ToLower() && game.OwnerUser.Name.ToLower() == user.Name.ToLower());
                return userGame;
            }
        }

        public void DeletePublishedGameByUser(Game gameToDelete)
        {
            IUserController userController = UserController.Instance;

            lock (_padlock)
            {
                if (_games == null)
                    throw new InvalidGameException();
                userController.DeleteGameFromAllUsers(gameToDelete);
                _games.Remove(gameToDelete);
            }
        }

        public void ModifyGame(Game gameToModify, Game newGame)
        {
            lock (_padlock)
            {
                if (gameToModify == null)
                    throw new InvalidGameException();
                gameToModify.Name = (newGame.Name == "") ? gameToModify.Name : newGame.Name;
                gameToModify.Genre = (newGame.Genre == "") ? gameToModify.Genre : newGame.Genre;
                gameToModify.Synopsis = (newGame.Synopsis == "") ? gameToModify.Synopsis : newGame.Synopsis;
                gameToModify.PathToPhoto = (newGame.PathToPhoto == "") ? gameToModify.PathToPhoto : newGame.PathToPhoto;
            }
        }
    }
}