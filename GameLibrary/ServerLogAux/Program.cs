using CommonLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace ServerLogAux
{
    class Program
    {
        static void Main(string[] args)
        {
            using var channel = new ConnectionFactory() { HostName = "localhost" }.CreateConnection().CreateModel();
            channel.QueueDeclare(queue: "log_queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            Console.WriteLine(" [x] Received log level Hola");
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var log = JsonSerializer.Deserialize<GameLogModel>(message);
                Console.WriteLine(" [x] Received log level [{0}], message [{1}]", log.User, log.Game);
            };
            channel.BasicConsume(queue: "log_queue",
                autoAck: true,
                consumer: consumer);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
