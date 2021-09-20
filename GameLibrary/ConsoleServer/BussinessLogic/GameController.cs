using ConsoleServer.Domain;
using ConsoleServer.Utils;
using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.BussinessLogic
{
    public class GameController
    {
        private static readonly object padlock = new object();
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
                lock (padlock)
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
            //Todo validar unico (?
            games.Add(gameToAdd);
        }

        public string GetGames()
        {
            return GameListToString(games);
        }

        public Game GetGame(string gameName)
        {
            if (games.Exists(game => game.Name.ToLower() == gameName.ToLower()))
                return games.Find(game => game.Name.ToLower() == gameName.ToLower());
            throw new InvalidGameException();
        }

        public void AddReview(string gameName, Review newReview)
        {
            Game gameToReview = GetGame(gameName);
            gameToReview.AddReview(newReview);
        }

        internal string GetGamesFiltered(string rawData)
        {
            string[] gamesFilters = rawData.Split('%');
            string gameName = gamesFilters[0].ToLower();
            string genre = gamesFilters[1].ToLower();
            int rating = Int32.Parse(gamesFilters[2]);

            List<Game> filteredGames = games.FindAll(game => game.Name.ToLower().Contains(gameName) || game.Genre.ToLower().Contains(genre)
                                                       || game.Rating == rating);
            string filteredGamesResult = GameListToString(filteredGames);
            return filteredGamesResult;
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
    }
}