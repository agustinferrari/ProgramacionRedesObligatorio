using System;
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
        private readonly IDictionary<string, IDictionary<string, List<GameLogModel>>> _userGameLog;
        private readonly IDictionary<string, IDictionary<DateTime, List<GameLogModel>>> _gameDateLog;
        private readonly IDictionary<string, List<GameLogModel>> _dateLog;
        private int _idLog = 1;

        private Games()
        {
            _logs = new List<GameLogModel>();
            _userGameLog = new Dictionary<string, IDictionary<string, List<GameLogModel>>>();
            _gameDateLog = new Dictionary<string, IDictionary<DateTime, List<GameLogModel>>>();
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
                    return GetUserGameLogs(filters);
                if (filters.User != "")
                    return GetUserLogs(filters.User);
                if (filters.Game != "")
                    return GetGameLogs(filters.Game);
                return _logs;
            }
        }

        public void AddGameLog(GameLogModel gameToAdd)
        {
            lock (_padlock)
            {
                gameToAdd.Id = _idLog++;
                _logs.Add(gameToAdd);
                AddUserGameLog(gameToAdd);
                AddGameDateLog(gameToAdd);
            }
        }

        private void AddGameDateLog(GameLogModel log)
        {
            string game = log.Game;
            DateTime date = log.Date.Date;
            IDictionary<DateTime, List<GameLogModel>> compositeLogs = null;
            if (!_gameDateLog.ContainsKey(game))
            {
                compositeLogs = new Dictionary<DateTime, List<GameLogModel>>();
                _gameDateLog.Add(game, null);
            }
            else
                compositeLogs = _gameDateLog[game];
            _gameDateLog[game] = UpadateCompositeGameLog(compositeLogs, date, log);
        }

        private IDictionary<DateTime, List<GameLogModel>> UpadateCompositeGameLog(IDictionary<DateTime, List<GameLogModel>> compositeGameLog, DateTime date, GameLogModel log)
        {
            List<GameLogModel> dateLogs = null;
            if (compositeGameLog == null)
                compositeGameLog = new Dictionary<DateTime, List<GameLogModel>>();

            if (!compositeGameLog.ContainsKey(date))
            {
                dateLogs = new List<GameLogModel>();
                compositeGameLog.Add(date, null);
            }
            else
                dateLogs = compositeGameLog[date];
            dateLogs.Add(log);
            compositeGameLog[date] = dateLogs;
            return compositeGameLog;
        }

        private void AddUserGameLog(GameLogModel log)
        {
            string user = log.User;
            string game = log.Game;
            IDictionary<string, List<GameLogModel>> compositeLogs = null;
            if (!_userGameLog.ContainsKey(user))
            {
                compositeLogs = new Dictionary<string, List<GameLogModel>>();
                _userGameLog.Add(user, null);
            }
            else
                compositeLogs = _userGameLog[user];
            _userGameLog[user] = UpadateCompositeGameLog(compositeLogs, game, log);
        }

        private IDictionary<string, List<GameLogModel>> UpadateCompositeGameLog(IDictionary<string, List<GameLogModel>> compositeGameLog, string game, GameLogModel log)
        {
            List<GameLogModel> gameLogs = null;
            if (compositeGameLog == null)
                compositeGameLog = new Dictionary<string, List<GameLogModel>>();
            if (game == null)
                game = "";

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

        private List<GameLogModel> GetUserLogs(string user)
        {
            if (_userGameLog != null && _userGameLog.ContainsKey(user))
            {
                List<GameLogModel> userLogs = _userGameLog[user].Values.SelectMany(x => x).ToList();
                return userLogs;
            }
            return null;
        }

        private List<GameLogModel> GetGameLogs(string game)
        {
            if (_gameDateLog != null && _gameDateLog.ContainsKey(game))
            {
                List<GameLogModel> userLogs = _gameDateLog[game].Values.SelectMany(x => x).ToList();
                return userLogs;
            }
            return null;
        }

        private List<GameLogModel> GetUserGameLogs(FilterModel filters)
        {
            if (_userGameLog != null && _userGameLog.ContainsKey(filters.User))
            {
                IDictionary<string, List<GameLogModel>> userLogs = _userGameLog[filters.User];
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