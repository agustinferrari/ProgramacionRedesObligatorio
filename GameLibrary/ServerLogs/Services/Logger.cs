using System;
using System.Threading;
using System.Threading.Tasks;
using CommonModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
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
            await _busControl.ReceiveAsync<GameModel>(Queue.ProcessingQueueName, x =>
            {
                Task.Run(() => { ReceiveItem(x); }, stoppingToken);
            });
        }

        private void ReceiveItem(GameModel gameModel)
        {
            _logger.LogInformation(PrintLog(gameModel));
            try
            {
                var context = Games.Instance;
                context.AddGameLog(gameModel);
                _logger.LogInformation($"Add 1 item");
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Exception {e.Message} -> {e.StackTrace}");
            }
        }

        private string PrintLog(GameModel gameModel)
        {
            string message = "User: " + gameModel.User;
            message += ", Accion: "+ gameModel.CommandConstant;
            message += ", Juego: "+ gameModel.Game == "" ? "": gameModel.Game;
            message += ", Completado: "+ (gameModel.Result ? "YES" : "NO");
            message += ", Date: "+ gameModel.Date;
            return message;
        }
    }
}