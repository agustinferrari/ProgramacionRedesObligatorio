#nullable enable
using System;

namespace LogsModels
{
    public class LogGameModel
    {
        private int _commandConstant;
        private string _user;
        private string _game;

        public int Id { get; set; }
        public int CommandConstant
        {
            get => _commandConstant;
            set
            {
                Command = CommandDictionary.ParseCommand(value);
                _commandConstant = value;
            }
        }
        public string? Command { get; set; }
        public string User
        {
            get => _user;
            set
            {
                if (value != null)
                    _user = value.ToLower();
            }
        }
        public string Game
        {
            get => _game;
            set
            {
                if (value != null)
                    _game = value.ToLower();
            }
        }
        public DateTime Date { get; set; }
        public bool Result { get; set; }

        public LogGameModel(int command)
        {
            Date = DateTime.Now;
            Result = false;
            CommandConstant = command;
        }

        public LogGameModel()
        {
        }
    }
}
