
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LogsModels;
using Common.Protocol;
using System.Collections.Generic;

namespace Server.Logic.LogManager
{

    class LogLogic
    {
        private static readonly object _padlock = new object();
        private static LogLogic _instance = null;
        private static IModel _channel = null;
        private List<int> _unwantedCommands = new List<int>{CommandConstants.GetGameDetails, CommandConstants.GetGameImage,
                                                        CommandConstants.ListFilteredGames, CommandConstants.ListGames,
                                                        CommandConstants.ListOwnedGames };

        private LogLogic()
        {
            _channel = new ConnectionFactory() { HostName = "localhost" }.CreateConnection().CreateModel();
            _channel.QueueDeclare(queue: "log_queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public static LogLogic Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new LogLogic();
                    }
                    return _instance;
                }
            }
        }

        public async Task SendLog(LogGameModel log)
        {
            if (!_unwantedCommands.Contains(log.CommandConstant))
            {
                string message = JsonSerializer.Serialize(log);
                try
                {
                    byte[] body = Encoding.UTF8.GetBytes(message);
                    _channel.BasicPublish(exchange: "",
                        routingKey: "log_queue",
                        basicProperties: null,
                        body: body);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

    }
}
