using System.Collections.Generic;
using System.Linq;
using CommonLog;
using ServerLogs.Models;

namespace ServerLogs.LogsStorage.GameLogs
{
    public class Games
    {
        private static readonly object _padlock = new object();
        private static Games _instance = null;
        private readonly List<GameLogModel> _logs;
        private readonly IDictionary<string, List<GameLogModel>> _userLogs;
        private readonly IDictionary<string, IDictionary<string, List<GameLogModel>>> _compositeLog;
        private int _idLog = 1;

        private Games()
        {
            _logs = new List<GameLogModel>();
            _userLogs = new Dictionary<string, List<GameLogModel>>();
        }

        public List<GameLogModel> GetLogs()
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

        public List<GameLogModel> GetLogs(FilterModel filters)
        {
            lock (_padlock)
                if (filters.User != "")
                    return GetUserLogs(filters.User);
                else
                    return _logs;
        }

        public void AddGameLog(GameLogModel gameToAdd)
        {
            lock (_padlock)
            {
                gameToAdd.Id = _idLog++;
                _logs.Add(gameToAdd);
                AddUserLog(gameToAdd);
            }
        }

        /*public GameLogModel DeleteLog(int id)
        {
            lock (_padlock)
                if (_logs != null)
                {
                    GameLogModel log = _logs.FirstOrDefault(g => g.Id == id);
                    _logs.Remove(log);
                    return log;
                }
            return null;
        }*/

        private void AddUserLog(GameLogModel log)
        {
            string user = log.User;
            List<GameLogModel> userLogs = null;
            if (!_userLogs.ContainsKey(user))
            {
                userLogs = new List<GameLogModel>();
                _userLogs.Add(user, userLogs);
            }
            else
                userLogs = _userLogs[user];
            userLogs.Add(log);
            _userLogs[user] = userLogs;
        }

        private List<GameLogModel> GetUserLogs(string user)
        {
            if (_userLogs != null && _userLogs.ContainsKey(user))
            {
                List<GameLogModel> userLogs = _userLogs[user];
                return userLogs;
            }
            return null;
        }
    }
}