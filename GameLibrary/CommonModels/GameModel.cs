#nullable enable
using System;

namespace CommonModels
{

    public class GameModel
    {
        public int Id { get; set; }
        public int CommandConstant { get; set; }
        public string User { get; set; }
        public string Game { get; set; }
        public DateTime Date { get; set; }
        public bool Result { get; set; }

        public GameModel(int command)
        {
            Date = DateTime.Now;
            Result = false;
            CommandConstant = command;
        }

        public GameModel()
        {
        }
    }
}
