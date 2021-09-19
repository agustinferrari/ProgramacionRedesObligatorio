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

        public string GetAllGames()
        {
            string result = "";
            for (int i = 0; i < games.Count; i++)
            {
                Game game = games[i];
                result += game.Name;
                if (i < games.Count - 1)
                    result += "\n";
            }
            return result;
        }

        public Game GetOneGame(string gameName)
        {
            if (games.Exists(game => game.Name == gameName))
                return games.Find(game => game.Name == gameName);
            throw new InvalidGameException();
        }
    }
}