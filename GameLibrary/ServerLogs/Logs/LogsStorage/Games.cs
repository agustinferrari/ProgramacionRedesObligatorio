using LogsModels;
using ServerLogs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerLogs.Logs.LogsStorage
{
    public class Games
    {
        private static readonly object _padlock = new object();
        private readonly IDictionary<string, IDictionary<DateTime, List<LogGameModel>>> _gameDateLog;
        private static Games _instance = null;

        private Games()
        {
            _gameDateLog = new Dictionary<string, IDictionary<DateTime, List<LogGameModel>>>();
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

        public void AddGameDateLog(LogGameModel log)
        {
            lock (_padlock)
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

        public List<LogGameModel> GetGameLogs(FilterModel filters)
        {
            lock (_padlock)
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
        }

    }
}
