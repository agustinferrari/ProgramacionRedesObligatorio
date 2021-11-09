namespace ServerLogs.Services.RabbitMQService
{
    public class Queue
    {
        public static string ProcessingQueueName { get; } = "log_queue"; 
    }
}