using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ServerLogs.Services.RabbitMQService
{
    public class RabbitBus : IBus
    {
        private readonly IModel _channel;

        internal RabbitBus(IModel channel)
        {
            _channel = channel;
        }


        public async Task ReceiveAsync<T>(string queue, Action<T> onMessage)
        {
            _channel.QueueDeclare(queue, false, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (s, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var item = JsonConvert.DeserializeObject<T>(message);
                onMessage(item);
                await Task.Yield();
            };
            _channel.BasicConsume(queue, true, consumer);
            await Task.Yield();
        }
    }
}