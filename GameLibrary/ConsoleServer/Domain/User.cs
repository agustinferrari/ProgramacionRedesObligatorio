using System;
using System.Collections.Generic;
using ConsoleServer.Utils.CustomExceptions;

namespace ConsoleServer.Domain
{
    public class User
    {
        public string Name { get; set; }
        public List<Game> OwnedGames { get; set; }


        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            return this.Name == ((User)obj).Name;
        }

        public void AddGame(Game game)
        {
            if (OwnedGames == null)
                OwnedGames = new List<Game>();
            if (!OwnedGames.Contains(game))
                OwnedGames.Add(game);
            else
                throw new GameAlreadyBoughtException();
        }
    }
}