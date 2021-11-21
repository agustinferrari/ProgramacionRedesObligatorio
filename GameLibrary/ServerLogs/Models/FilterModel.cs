using System;
namespace ServerLogs.Models
{
    public class FilterModel
    {
        private string _user;
        private string _game;
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
        public DateTime? Date { get; set; }
    }
}