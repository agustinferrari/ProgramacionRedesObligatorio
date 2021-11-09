using System;

namespace CommonLog
{

    public class GameLogModel
    {
        public int CommandConstant { get; set; }
        public string User { get; set; }
        public string Game { get; set; }
        public DateTime Date { get; set; }
        public bool Result { get; set; }

        public GameLogModel(int command)
        {
            Date = DateTime.Today;
            Result = false;
            CommandConstant = command;
        }

        public GameLogModel()
        {
        }
    }
}
