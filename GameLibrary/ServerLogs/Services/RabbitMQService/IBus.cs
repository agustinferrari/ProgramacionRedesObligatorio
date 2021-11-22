using System;
using System.Threading.Tasks;

namespace ServerLogs.Services.RabbitMQService
{
    public interface IBus
    {
        Task ReceiveAsync<T>(string queue, Action<T> onMessage);
    }
}