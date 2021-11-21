using System;
using System.Collections.Generic;
using System.Linq;
using LogsModels;
using ServerLogs.Models;

namespace ServerLogs.LogsStorage.GameLogs
{
    public class Games
    {
        private static readonly object _padlock = new object();
        private static Games _instance = null;
        private readonly List<LogGameModel> _logs;
        private readonly IDictionary<string, IDictionary<string, List<LogGameModel>>> _userGameLog;
        private readonly IDictionary<string, IDictionary<DateTime, List<LogGameModel>>> _gameDateLog;
        private readonly IDictionary<DateTime, List<LogGameModel>> _dateLog;
        private int _idLog = 1;

        private Games()
        {
            _logs = new List<LogGameModel>();
            _userGameLog = new Dictionary<string, IDictionary<string, List<LogGameModel>>>();
            _gameDateLog = new Dictionary<string, IDictionary<DateTime, List<LogGameModel>>>();
            _dateLog = new Dictionary<DateTime, List<LogGameModel>>();
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

        public List<LogGameModel> GetLogs(FilterModel filters)
        {
            lock (_padlock)
            {
                if (filters.User != null && filters.Game != null)
                    return GetUserGameLogs(filters);
                if (filters.User != null)
                    return GetUserLogs(filters);
                if (filters.Game != null)
                    return GetGameLogs(filters);
                if (filters.Date != null)
                {
                    DateTime convertedDateTime = filters.Date.Value;
                    return GetDateLogs(convertedDateTime);
                }

                return _logs;
            }
        }

        public void AddLog(LogGameModel gameToAdd)
        {
            lock (_padlock)
            {
                gameToAdd.Id = _idLog++;
                _logs.Add(gameToAdd);
                AddUserGameLog(gameToAdd);
                AddGameDateLog(gameToAdd);
                AddDateLog(gameToAdd);
            }
        }

        private void AddGameDateLog(LogGameModel log)
        {
            string game = log.Game;
            if (game != null)
            {
                DateTime date = log.Date.Date;
                IDictionary<DateTime, List<LogGameModel>> compositeLogs = null;
                if (!_gameDateLog.ContainsKey(game))
                {
                    compositeLogs = new Dictionary<DateTime, List<LogGameModel>>();
                    _gameDateLog.Add(game, null);
                }
                else
                    compositeLogs = _gameDateLog[game];
                _gameDateLog[game] = UpadateCompositeGameLog(compositeLogs, date, log);
            }
        }

        private IDictionary<DateTime, List<LogGameModel>> UpadateCompositeGameLog(IDictionary<DateTime, List<LogGameModel>> compositeGameLog, DateTime date, LogGameModel log)
        {
            List<LogGameModel> dateLogs = null;
            if (compositeGameLog == null)
                compositeGameLog = new Dictionary<DateTime, List<LogGameModel>>();

            if (!compositeGameLog.ContainsKey(date))
            {
                dateLogs = new List<LogGameModel>();
                compositeGameLog.Add(date, null);
            }
            else
                dateLogs = compositeGameLog[date];
            dateLogs.Add(log);
            compositeGameLog[date] = dateLogs;
            return compositeGameLog;
        }

        private void AddUserGameLog(LogGameModel log)
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

        private void AddDateLog(LogGameModel log)
        {
            DateTime date = log.Date.Date;
            List<LogGameModel> logs = null;
            if (!_dateLog.ContainsKey(date))
            {
                logs = new List<LogGameModel>();
                _dateLog.Add(date, null);
            }
            else
                logs = _dateLog[date];
            logs.Add(log);
            _dateLog[date] = logs;
        }

        private List<LogGameModel> GetUserLogs(FilterModel filters)
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

        private List<LogGameModel> GetGameLogs(FilterModel filters)
        {
            string game = filters.Game;
            if (_gameDateLog != null && _gameDateLog.ContainsKey(game))
            {
                List<LogGameModel> gameLogs = _gameDateLog[game].Values.SelectMany(x => x).ToList();
                if (filters.Date != null)
                {
                    DateTime convertedDateTime = filters.Date.Value.Date;
                    return gameLogs.Where(x => x.Date == convertedDateTime).ToList();
                }
                return gameLogs;
            }
            return null;
        }

        private List<LogGameModel> GetUserGameLogs(FilterModel filters)
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

        private List<LogGameModel> GetDateLogs(DateTime date)
        {
            date = date.Date;
            if (_dateLog != null && _dateLog.ContainsKey(date))
            {
                List<LogGameModel> userLogs = _dateLog[date];
                return userLogs;
            }
            return null;
        }
    }
}