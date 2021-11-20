using System;
using System.Threading.Tasks;
using CommonModels;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

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
        
        public async Task<string> GetUsers(string userAsking )
        {
            string userVerified = verifyUserAsking(userAsking);
            if (userVerified != null)
                return userVerified;
            var response =  await _client.GetUsersAsync(new UsersRequest(){ UserAsking = userAsking});
            return  response.Users;
        }

        public async Task<string> AddModifyUser(string userAsking, UserModel userToAdd)
        {
            string userVerified = verifyUserAsking(userAsking);
            if (userVerified != null)
                return userVerified;
            var response =  await _client.AddUserAsync(
                new AddModifyUserRequest(){UserAsking = userAsking, UserToAddModify = userToAdd.Name});
            return response.Response;
        }

        public async Task<string> DeleteUser(string userAsking , string userToDelete)
        {
            string userVerified = verifyUserAsking(userAsking);
            if (userVerified != null)
                return userVerified;
            var response =  await _client.DeleteUserAsync(
                new DeleteUserRequest(){ UserAsking = userAsking,UserToDelete = userToDelete});
            return response.DeletedUser;
        }

        public async Task<string> BuyGame(string userAsking, string gameToBuy)
        {
            string userVerified = verifyUserAsking(userAsking);
            if (userVerified != null)
                return userVerified;
            var response =  await _client.BuyGameAsync(
                new BuyGameRequest(){UserAsking = userAsking, GameToBuy = gameToBuy});
            return response.Response;
        }
        
        public async Task<string> ModifyUser(string userAsking, UserModel newUserName)
        {
            string userVerified = verifyUserAsking(userAsking);
            if (userVerified != null)
                return userVerified;
            var response =  await _client.ModifyUserAsync(
                new AddModifyUserRequest(){UserAsking = userAsking, UserToAddModify = newUserName.Name});
            return response.Response;
        }

        private string verifyUserAsking(string userAsking)
        {
            if (userAsking == null)
                return "Por favor ingrese su nombre de usuario para realizar este pedido";
            return null;
        }

   
    }
}