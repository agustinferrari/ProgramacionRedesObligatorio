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
    }
}
