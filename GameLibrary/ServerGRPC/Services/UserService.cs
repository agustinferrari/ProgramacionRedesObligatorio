
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;


namespace ServerGRPC.Services
{
    public class UserService
    {
        
        public Task<NewUserReply> AddUser(NewUserRequest request, ServerCallContext context)
        {
            //_logger.LogInformation("Received request with data: " + request.Name);
            return Task.FromResult(new NewUserReply
            {
                Message = "Hola cliente: " + request.Name
            });
        }
    
    }
}