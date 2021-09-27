using ConsoleServer.BussinessLogic.Interfaces;
using ConsoleServer.Domain;
using ConsoleServer.Utils;
using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.BussinessLogic
{
    public class GameController : IGameController
    {
        private static readonly object _padlock = new object();
        private static GameController _instance = null;
        private List<Game> games;

        private GameController()
        {
            games = new List<Game>();
            CatalogueLoader.AddGames(this);
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
                if (games != null && games.Exists(game => game.Name.ToLower() == gameToAdd.Name.ToLower()))
                    throw new GameAlreadyAddedException();
                else
                    games.Add(gameToAdd);
            }
        }

        public string GetGames()
        {
            lock (_padlock)
                if (games != null)
                    return GameListToString(games);
            throw new InvalidGameException();
        }

        public Game GetGame(string gameName)
        {
            lock (_padlock)
            {
                if (games != null && games.Exists(game => game.Name.ToLower() == gameName.ToLower()))
                    return games.Find(game => game.Name.ToLower() == gameName.ToLower());
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
                if (games != null)
                {
                    List<Game> filteredGames = games.FindAll(game => game.Name.ToLower().Contains(gameName) && game.Genre.ToLower().Contains(genre)
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
                if (games == null)
                    return null;
                Game userGame = games.Find(game => game.Name.ToLower() == gameName.ToLower() && game.OwnerUser.Name.ToLower() == user.Name.ToLower());
                return userGame;
            }
        }

        public void DeletePublishedGameByUser(Game gameToDelete)
        {
            if (games == null)
                throw new InvalidGameException();

            lock (_padlock)
                games.Remove(gameToDelete);
        }

        public void ModifyGame(Game gameToModify, Game newGame)
        {
            if (gameToModify == null)
                throw new InvalidGameException();
            lock (_padlock)
            {
                gameToModify.Name = (newGame.Name == "") ? gameToModify.Name : newGame.Name;
                gameToModify.Genre = (newGame.Genre == "") ? gameToModify.Genre : newGame.Genre;
                gameToModify.Synopsis = (newGame.Synopsis == "") ? gameToModify.Synopsis : newGame.Synopsis;
                gameToModify.PathToPhoto = (newGame.PathToPhoto == "") ? gameToModify.PathToPhoto : newGame.PathToPhoto;
            }
        }
    }
}