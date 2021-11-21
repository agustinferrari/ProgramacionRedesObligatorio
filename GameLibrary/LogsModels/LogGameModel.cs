#nullable enable
using System;

namespace LogsModels
{
    public class LogGameModel
    {
        private int _commandConstant;
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
        public string User { get; set; }
        public string Game { get; set; }
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
