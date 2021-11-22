using LogsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerLogs.Logs.LogsStorage
{
    public class Dates
    {
        private static readonly object _padlock = new object();
        private readonly IDictionary<DateTime, List<LogGameModel>> _dateLog;
        private static Dates _instance = null;

        private Dates()
        {
            _dateLog = new Dictionary<DateTime, List<LogGameModel>>();
        }

        public static Dates Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Dates();
                    }
                    return _instance;
                }
            }
        }

        public void AddDateLog(LogGameModel log)
        {
            lock (_padlock)
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
        }

        public List<LogGameModel> GetDateLogs(DateTime date)
        {
            lock (_padlock)
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
}
