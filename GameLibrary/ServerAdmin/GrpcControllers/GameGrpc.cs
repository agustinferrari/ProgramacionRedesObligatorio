using System;
using System.Collections.Generic;
using CommonModels;
using Grpc.Net.Client;

namespace ServerAdmin.GrpcControllers
{
    public class GameGrpc
    {
        // public GameGrpc()
        // {
        //     AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport",true);
        //     Console.WriteLine("Starting gRPC client example......");
        //     var channel = GrpcChannel.ForAddress("http://localhost:5001");
        //     var client = new GameProto.GameProtoClient(channel);
        //     
        //     var response =  await client.GetGames(new GamesRequest(){Command = "GetGames", User = "Juan"});
        //     Console.WriteLine("Respuesta: " + response.Message);
        //     
        //     var gameList = await client.GetGames(new UserRequest());
        //     
        //     foreach (var userListUser in gameList.Games)
        //     {
        //         Console.WriteLine("User Name: " + userListUser.UserName + ", User Age: " + userListUser.UserAge);
        //     }
        //     
        //     Console.ReadLine();
        // }
        // public IEnumerable<GameModel> GetGames()
        // {
        //     
        // }
    }
}