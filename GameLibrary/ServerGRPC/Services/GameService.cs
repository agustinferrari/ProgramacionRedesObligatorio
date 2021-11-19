
using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ServerGRPC.Domain;


namespace ServerGRPC.Services
{
    public class GameService : GameProto.GameProtoBase
    {
        private readonly ILogger<GameService> _logger;

        public GameService(ILogger<GameService> logger)
        {
            _logger = logger;
        }
        
        public override Task<GamesReply> GetGames(GamesRequest request, ServerCallContext context)
        {
            //_logger.LogInformation("Received request with data: " + request.Name);
            System.Diagnostics.Debug.WriteLine
                ("Llegue a serviesGame !!!!!!!!!!!!!!!");         
            System.Diagnostics.Debug.WriteLine
                ("Received request with data: " + request.User);

            return Task.FromResult(new GamesReply
            {
                Games = "Hola cliente: " + request.Command
            });
        }
    
    }
}