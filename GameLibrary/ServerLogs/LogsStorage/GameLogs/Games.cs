using System.Collections.Generic;
using System.Linq;
using LogsModels;

namespace ServerLogs.LogsStorage.GameLogs
{
    public class Games
    {
        private static readonly object _padlock = new object();
        private static Games _instance = null;
        private readonly List<LogGameModel> _logs;
        private int _idLog = 1;

        private Games()
        {
            _logs = new List<LogGameModel>();
        }

        public List<LogGameModel> GetLogs()
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

        public LogGameModel GetLog(int id)
        {
            lock (_padlock)
                if (_logs != null)
                    return _logs.FirstOrDefault(g => g.Id == id);
            return null;
        }
        
        public void AddGameLog(LogGameModel logGameToAdd)
        {
            lock (_padlock)
            {
                logGameToAdd.Id = _idLog++;
                _logs.Add(logGameToAdd);
            }
        }

        public LogGameModel DeleteLog(int id)
        {
            lock (_padlock)
                if (_logs != null)
                {
                    LogGameModel log = _logs.FirstOrDefault(g => g.Id == id);
                    _logs.Remove(log);
                    return log;
                }
            return null;
        }
    }
}