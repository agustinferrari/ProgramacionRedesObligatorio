using System;
using System.Threading.Tasks;
using CommonModels;
using Grpc.Net.Client;

namespace ServerAdmin.ServicesGrpc
{
    public class GameGrpc
    {
        private GameProto.GameProtoClient _client;
        public GameGrpc()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport",true);
            Console.WriteLine("Starting gRPC client ......");
            var channel = GrpcChannel.ForAddress("http://localhost:5004");
            _client = new GameProto.GameProtoClient(channel);
        }
        
        public async Task<string> GetGames(string user)
        {
            var response =  await _client.GetGamesAsync(new GamesRequest(){ User = user});
            return response.Games;
        }

        public async Task<string> AddGame(GameModel model)
        {
            AddUpdateGameRequest request = new AddUpdateGameRequest()
            {
                Name = model.Name,
                Genre = model.Genre,
                Synopsis = model.Synopsis,
                OwnerUserName = model.OwnerUserName,
                PathToPhoto = model.PathToPhoto
            };

            var response = await _client.AddModifyGamesAsync(request);
            return response.Response;
        }

        public async Task<string> DeleteGame(string user, string game)
        {
            var response =  await _client.DeleteGameAsync(new DeleteGameRequest(){ User = user, GameToDelete = game});
             return response.DeletedGame;
        }
    }
}