using System;
using System.Threading;
using System.Threading.Tasks;
using CommonLog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                using (var scope = _serviceProvider.CreateScope()) // Creamos un contexto de invocacion
                {
                    // var db = new TodoContext(scope.ServiceProvider.GetRequiredService<DbContextOptions<TodoContext>>());
                    // db.TodoItems.Add(gameLogModel);
                    // var addedItems = db.SaveChanges();
                    // _logger.LogInformation($"Add {addedItems} items");
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Exception {e.Message} -> {e.StackTrace}");
            }
        }
    }
}