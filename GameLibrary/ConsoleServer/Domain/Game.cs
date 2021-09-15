using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Domain
{
    public class Game
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Genre { get; set; }
        public string Synopsis { get; set; }
        public string PathToPhoto { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
