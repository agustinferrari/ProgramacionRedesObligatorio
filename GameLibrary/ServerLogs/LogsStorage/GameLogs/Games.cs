using System.Collections.Generic;
using System.Linq;
using CommonModels;

namespace ServerLogs.LogsStorage.GameLogs
{
    public class Games
    {
        private static readonly object _padlock = new object();
        private static Games _instance = null;
        private readonly List<GameModel> _logs;
        private int _idLog = 1;

        private Games()
        {
            _logs = new List<GameModel>();
        }

        public List<GameModel> GetLogs()
        {
            return _logs;
        }

        public static Games Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Games();
                    }
                    return _instance;
                }
            }
        }

        public GameModel GetLog(int id)
        {
            lock (_padlock)
                if (_logs != null)
                    return _logs.FirstOrDefault(g => g.Id == id);
            return null;
        }
        
        public void AddGameLog(GameModel gameToAdd)
        {
            lock (_padlock)
            {
                gameToAdd.Id = _idLog++;
                _logs.Add(gameToAdd);
            }
        }

        public GameModel DeleteLog(int id)
        {
            lock (_padlock)
                if (_logs != null)
                {
                    GameModel log = _logs.FirstOrDefault(g => g.Id == id);
                    _logs.Remove(log);
                    return log;
                }
            return null;
        }
    }
}