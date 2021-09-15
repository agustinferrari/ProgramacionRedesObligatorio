using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.BussinessLogic
{
    public class GameController
    {
        private static GameController _instance;
        private List<Game> games;

        private GameController()
        {
            games = new List<Game>();
        }

        public static GameController GetInstance()
        {
            if (_instance == null)
                _instance = new GameController();
            return _instance;
        }

        public void AddGame(Game gameToAdd)
        {
            //Todo validar unico (?
            games.Add(gameToAdd);
        }

        public string GetGames()
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

        public Game GetGame(string gameName)
        {
            if (games.Exists(game => game.Name == gameName))
                return games.Find(game => game.Name == gameName);
            throw new InvalidGameException();
        }
    }
}
