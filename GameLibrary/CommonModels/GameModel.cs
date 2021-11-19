#nullable enable
using System;

namespace CommonModels
{
    public class GameModel
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Genre { get; set; }
        public string Synopsis { get; set; }
        public string PathToPhoto { get; set; }
        public string OwnerUserName { get; set; }
        
        public GameModel()
        {
        }
    }
}
