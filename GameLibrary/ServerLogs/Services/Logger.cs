using System;
using System.Threading;
using System.Threading.Tasks;
using LogsModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerLogs.LogsStorage.GameLogs;
using ServerLogs.Services.RabbitMQService;

namespace ServerLogs.Services
{
    public class Logger : BackgroundService
    {
        private readonly ILogger<Logger> _logger;
        private readonly IBus _busControl;
        private readonly IServiceProvider _serviceProvider;
        
        public Logger(ILogger<Logger> logger, IServiceProvider serviceProvider){
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

        private void ReceiveItem(LogGameModel logGameModel)
        {
            _logger.LogInformation(PrintLog(logGameModel));
            try
            {
                var context = Games.Instance;
                context.AddGameLog(logGameModel);
                _logger.LogInformation($"Add 1 item");
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Exception {e.Message} -> {e.StackTrace}");
            }
        }

        private string PrintLog(LogGameModel logGameModel)
        {
            string message = "User: " + logGameModel.User;
            message += ", Accion: "+ logGameModel.CommandConstant;
            message += ", Juego: "+ logGameModel.Game == "" ? "": logGameModel.Game;
            message += ", Completado: "+ (logGameModel.Result ? "YES" : "NO");
            message += ", Date: "+ logGameModel.Date;
            return message;
        }
    }
}