using ConsoleServer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.BussinessLogic
{
    public class GameController
    {
        List<Game> games;

        public GameController()
        {
            games = new List<Game>();
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
    }
}
