using System;
using System.Collections.Generic;
using System.Text;

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

        internal void AddGame(Game game)
        {
            //Ver si fijarnos si ya lo tiene
            if (OwnedGames == null)
                OwnedGames = new List<Game>();
            if (!OwnedGames.Contains(game))
                OwnedGames.Add(game);
        }
    }
}