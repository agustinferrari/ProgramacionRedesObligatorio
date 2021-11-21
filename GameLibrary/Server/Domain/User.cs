using System;
using System.Collections.Generic;
using Server.Utils.CustomExceptions;

namespace Server.Domain
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
            if (!OwnedGames.Contains(game))
                OwnedGames.Add(game);
            else
                throw new GameAlreadyBoughtException();
        }
        
        public void DeleteGame(Game game)
        {
            if (OwnedGames.Contains(game))
                OwnedGames.Remove(game);
            else
                throw new InvalidDeleteGameForUserException();
        }
    }
}