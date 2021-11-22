using System;
using System.Collections.Generic;
using System.Linq;
using LogsModels;
using ServerLogs.Logs.LogsStorage;
using ServerLogs.Models;

namespace ServerLogs.Logs
{
    public class LogsLogic
    {
        private static readonly object _padlock = new object();
        private static LogsLogic _instance = null;
        private readonly List<LogGameModel> _logs;
        private readonly Games _gameLogs;
        private readonly Users _userLogs;
        private readonly Dates _dateLogs;
        private int _idLog = 1;

        private LogsLogic()
        {
            _gameLogs = Games.Instance;
            _userLogs = Users.Instance;
            _dateLogs = Dates.Instance;
            _logs = new List<LogGameModel>();
        }

        public List<LogGameModel> GetLogs()
        {
            return _logs;
        }

        public static LogsLogic Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new LogsLogic();
                    }
                    return _instance;
                }
            }
        }

        public List<LogGameModel> GetLogs(FilterModel filters)
        {

            if (filters.User != null && filters.Game != null)
                return _userLogs.GetUserGameLogs(filters);
            if (filters.User != null)
                return _userLogs.GetUserLogs(filters);
            if (filters.Game != null)
                return _gameLogs.GetGameLogs(filters);
            if (filters.Date != null)
            {
                DateTime convertedDateTime = filters.Date.Value;
                return _dateLogs.GetDateLogs(convertedDateTime);
            }

            lock (_padlock)
            {
                return _logs;
            }
        }

        public void AddLog(LogGameModel gameToAdd)
        {
            lock (_padlock)
            {
                gameToAdd.Id = _idLog++;
                _logs.Add(gameToAdd);
            }
            _userLogs.AddUserGameLog(gameToAdd);
            _gameLogs.AddGameDateLog(gameToAdd);
            _dateLogs.AddDateLog(gameToAdd);

        }
    }
}