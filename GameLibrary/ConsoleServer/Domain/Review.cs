using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Domain
{
    public class Review
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public User User { get; set; }
    }
}
