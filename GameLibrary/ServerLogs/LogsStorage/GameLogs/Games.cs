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
            _compositeLog = new Dictionary<string, IDictionary<string, List<GameLogModel>>>();
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
            {
                if (filters.User != "" && filters.Game != "")
                    return GetCompositeLogs(filters);
                if (filters.User != "")
                    return GetUserLogs(filters.User);
                return _logs;
            }
        }

        public void AddGameLog(GameLogModel gameToAdd)
        {
            lock (_padlock)
            {
                gameToAdd.Id = _idLog++;
                _logs.Add(gameToAdd);
                AddUserLog(gameToAdd);
                AddCompositeLog(gameToAdd);
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
                _userLogs.Add(user, null);
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

        private void AddCompositeLog(GameLogModel log)
        {
            string user = log.User;
            string game = log.Game;
            IDictionary<string, List<GameLogModel>> compositeLogs = null;
            if (!_compositeLog.ContainsKey(user))
            {
                compositeLogs = new Dictionary<string, List<GameLogModel>>();
                _compositeLog.Add(user, null);
            }
            else
                compositeLogs = _compositeLog[user];
            _compositeLog[user] = UpadateCompositeGameLog(compositeLogs, game, log);
        }

        private IDictionary<string, List<GameLogModel>> UpadateCompositeGameLog(IDictionary<string, List<GameLogModel>> compositeGameLog, string game, GameLogModel log)
        {
            List<GameLogModel> gameLogs = null;
            if (compositeGameLog == null)
                compositeGameLog = new Dictionary<string, List<GameLogModel>>();

            if (!compositeGameLog.ContainsKey(game))
            {
                gameLogs = new List<GameLogModel>();
                compositeGameLog.Add(game, null);
            }
            else
                gameLogs = compositeGameLog[game];
            gameLogs.Add(log);
            compositeGameLog[game] = gameLogs;
            return compositeGameLog;
        }

        private List<GameLogModel> GetCompositeLogs(FilterModel filters)
        {
            if (_compositeLog != null && _compositeLog.ContainsKey(filters.User))
            {
                IDictionary<string, List<GameLogModel>> userLogs = _compositeLog[filters.User];
                if (userLogs != null && userLogs.ContainsKey(filters.Game))
                {
                    List<GameLogModel> gameLogs = userLogs[filters.Game];
                    return gameLogs;
                }
                return null;
            }
            return null;
        }
    }
}