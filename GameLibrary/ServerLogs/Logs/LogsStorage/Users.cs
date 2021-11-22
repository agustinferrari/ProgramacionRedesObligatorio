using LogsModels;
using ServerLogs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerLogs.Logs.LogsStorage
{
    public class Users
    {
        private static readonly object _padlock = new object();
        private readonly IDictionary<string, IDictionary<string, List<LogGameModel>>> _userGameLog;
        private static Users _instance = null;

        private Users()
        {
            _userGameLog = new Dictionary<string, IDictionary<string, List<LogGameModel>>>();
        }

        public static Users Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Users();
                    }
                    return _instance;
                }
            }
        }

        public void AddUserGameLog(LogGameModel log)
        {
            lock (_padlock)
            {
                string user = log.User;
                string game = log.Game;
                IDictionary<string, List<LogGameModel>> compositeLogs = null;
                if (!_userGameLog.ContainsKey(user))
                {
                    compositeLogs = new Dictionary<string, List<LogGameModel>>();
                    _userGameLog.Add(user, null);
                }
                else
                    compositeLogs = _userGameLog[user];
                _userGameLog[user] = UpadateCompositeGameLog(compositeLogs, game, log);
            }
        }

        private IDictionary<string, List<LogGameModel>> UpadateCompositeGameLog(IDictionary<string, List<LogGameModel>> compositeGameLog, string game, LogGameModel log)
        {
            List<LogGameModel> gameLogs = null;
            if (compositeGameLog == null)
                compositeGameLog = new Dictionary<string, List<LogGameModel>>();
            if (game == null)
                game = "";

            if (!compositeGameLog.ContainsKey(game))
            {
                gameLogs = new List<LogGameModel>();
                compositeGameLog.Add(game, null);
            }
            else
                gameLogs = compositeGameLog[game];
            gameLogs.Add(log);
            compositeGameLog[game] = gameLogs;
            return compositeGameLog;
        }

        public List<LogGameModel> GetUserLogs(FilterModel filters)
        {
            lock (_padlock)
            {
                string user = filters.User;
                if (_userGameLog != null && _userGameLog.ContainsKey(user))
                {
                    List<LogGameModel> userLogs = _userGameLog[user].Values.SelectMany(x => x).ToList();
                    if (filters.Date != null)
                    {
                        DateTime convertedDateTime = filters.Date.Value.Date;
                        return userLogs.Where(x => x.Date == convertedDateTime).ToList();
                    }
                    return userLogs;
                }
                return null;
            }
        }

        public List<LogGameModel> GetUserGameLogs(FilterModel filters)
        {
            lock (_padlock)
            {
                if (_userGameLog != null && _userGameLog.ContainsKey(filters.User))
                {
                    IDictionary<string, List<LogGameModel>> userLogs = _userGameLog[filters.User];
                    if (userLogs != null && userLogs.ContainsKey(filters.Game))
                    {
                        List<LogGameModel> gameLogs = userLogs[filters.Game];
                        if (filters.Date != null)
                        {
                            DateTime convertedDateTime = filters.Date.Value;
                            return gameLogs.Where(x => x.Date == convertedDateTime).ToList();
                        }
                        return gameLogs;
                    }
                    return null;
                }
                return null;
            }
        }
    }
}
