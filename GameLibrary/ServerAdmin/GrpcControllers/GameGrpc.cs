using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonModels;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using ServerAdmin.Controllers;

namespace ServerAdmin.GrpcControllers
{
    public class GameGrpc
    {
        private readonly ILogger<GameGrpc> _logger;
        public GameGrpc()
        {
           
        }
        public  async Task<string> GetGames()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport",true);
            Console.WriteLine("Starting gRPC client ......");
            var channel = GrpcChannel.ForAddress("http://localhost:5004");
            var client = new GameProto.GameProtoClient(channel);
            
            //falta hacer await??
            var response =  await client.GetGamesAsync(new GamesRequest(){ User = "Juan", Command = "GetGames"});
            Console.WriteLine("Respuesta: " + response.Games);
            
            // var gameList =  client.GetGames(new UserRequest());
            
            // foreach (var userListUser in gameList.Games)
            // {
            //     Console.WriteLine("User Name: " + userListUser.UserName + ", User Age: " + userListUser.UserAge);
            // }
            return "Respuesta: " + response.Games;
        }
    }
}