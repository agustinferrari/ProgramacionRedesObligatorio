
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;


namespace ServerGRPC.Services
{
    public class GameService
    {
        
        public Task<GamesReply> Get(GamesRequest request, ServerCallContext context)
        {
            //_logger.LogInformation("Received request with data: " + request.Name);
            return Task.FromResult(new GamesReply
            {
                Games = "Hola cliente: " + request.Command
            });
        }
    
    }
}