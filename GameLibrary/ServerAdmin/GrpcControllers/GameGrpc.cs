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
        //     var client = new Greeter.GreeterClient(channel);
        //     var response =  await client.SayHelloAsync(new HelloRequest{Name = "Hola"});
        //     Console.WriteLine("Respuesta: " + response.Message);
        //     
        //     Console.WriteLine("Ingrese un numero:");
        //     var numberParam = int.Parse(Console.ReadLine());
        //     var numberResponse = await client.GiveMeANumberAsync(new NumberRequest {NumberParameter = numberParam});
        //     Console.WriteLine("Respuesta del number: " + numberResponse.NumberResult);
        //
        //     var userList = await client.GiveMeUsersAsync(new UserRequest());
        //     
        //     foreach (var userListUser in userList.Users)
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