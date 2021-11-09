using Common.Protocol;
using CommonLog;
using ConsoleServer.Domain;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleServer.Logic.LogManager
{

    class LogLogic
    {
        private static readonly object _padlock = new object();
        private static LogLogic _instance = null;
        private static IModel _channel = null;

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

        public async void LogUserGame(string userName, Game game, int commandConstant)
        {
            GameLogModel log = new GameLogModel();
            log.User = userName;
            log.Game = game.Name;
            log.CommandConstant = commandConstant;
            string stringLog = JsonSerializer.Serialize(log);
            bool result = await SendMessage(stringLog);
        }

        private static Task<bool> SendMessage(string message)
        {
            bool returnVal;
            try
            {
                byte[] body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(exchange: "",
                    routingKey: "log_queue",
                    basicProperties: null,
                    body: body);
                returnVal = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                returnVal = false;
            }

            return Task.FromResult(returnVal);
        }
    }
}
