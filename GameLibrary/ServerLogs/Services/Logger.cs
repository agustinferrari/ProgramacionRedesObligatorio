using System;
using System.Threading;
using System.Threading.Tasks;
using CommonLog;
using Microsoft.Extensions.DependencyInjection;
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
            await _busControl.ReceiveAsync<GameLogModel>(Queue.ProcessingQueueName, x =>
            {
                Task.Run(() => { ReceiveItem(x); }, stoppingToken);
            });
        }

        private void ReceiveItem(GameLogModel gameLogModel)
        {
            _logger.LogInformation("Name: "+gameLogModel.Game+", Complete: {0}",gameLogModel.Result ? "YES":"NO");
            try
            {
                var context = Games.Instance;
                context.AddGameLog(gameLogModel);
                _logger.LogInformation($"Add 1 items");
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Exception {e.Message} -> {e.StackTrace}");
            }
        }
    }
}