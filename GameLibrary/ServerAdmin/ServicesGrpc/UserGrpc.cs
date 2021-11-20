using System;
using System.Threading.Tasks;
using CommonModels;
using Grpc.Net.Client;

namespace ServerAdmin.ServicesGrpc
{
    public class UserGrpc
    {
        private UserProto.UserProtoClient _client;
        public UserGrpc()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport",true);
            Console.WriteLine("Starting gRPC client ......");
            var channel = GrpcChannel.ForAddress("http://localhost:5004");
            _client = new UserProto.UserProtoClient(channel);
        }
        
        public async Task<string> GetUsers(string user)
        {
            var response =  await _client.GetUsersAsync(new UsersRequest(){ User = user});
            return  response.Users;
        }

        public async Task<string> AddModifyUser(UserModel user)
        {
            var response =  await _client.AddModifyUsersAsync(new AddModifyUserRequest(){ Name = user.Name});
            return response.Response;
        }
        
    }
}