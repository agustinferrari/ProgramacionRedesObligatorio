using System;
using System.Threading;
using System.Threading.Tasks;
using LogsModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerLogs.Logs;
using ServerLogs.Services.RabbitMQService;

namespace ServerLogs.Services
{
    public class Logger : BackgroundService
    {
        private readonly ILogger<Logger> _logger;
        private readonly IBus _busControl;
        private readonly IServiceProvider _serviceProvider;

        public Logger(ILogger<Logger> logger, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _busControl = RabbitHutch.CreateBus();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _busControl.ReceiveAsync<LogGameModel>(Queue.ProcessingQueueName, x =>
            {
                Task.Run(() => { ReceiveItem(x); }, stoppingToken);
            });
        }

        private void ReceiveItem(LogGameModel gameLogModel)
        {
            _logger.LogInformation(PrintLog(gameLogModel));
            try
            {
                var context = LogsLogic.Instance;
                gameLogModel.Date = gameLogModel.Date.Date;
                context.AddLog(gameLogModel);
                _logger.LogInformation($"Add 1 item");
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Exception {e.Message} -> {e.StackTrace}");
            }
        }

        private string PrintLog(LogGameModel gameLogModel)
        {
            string message = "User: " + gameLogModel.User;
            message += ", Accion: " + gameLogModel.CommandConstant;
            message += ", Juego: " + gameLogModel.Game == "" ? "" : gameLogModel.Game;
            message += ", Completado: " + (gameLogModel.Result ? "YES" : "NO");
            message += ", Date: " + gameLogModel.Date;
            return message;
        }
    }
}