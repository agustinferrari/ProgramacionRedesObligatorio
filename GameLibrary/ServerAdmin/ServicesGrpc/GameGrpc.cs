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
        
        public async Task<string> GetGames(string userAsking)
        {
            if (userAsking == null)
                return "Por favor ingrese su nombre de usuario para realizar este pedido";
            var response =  await _client.GetGamesAsync(new GamesRequest(){ User = userAsking});
            return response.Games;
        }

        public async Task<string> AddGame(GameModel model)
        {
            AddGameRequest request = new AddGameRequest()
            {
                Name = model.Name,
                Genre = model.Genre,
                Synopsis = model.Synopsis,
                OwnerUserName = model.OwnerUserName,
                PathToPhoto = model.PathToPhoto
            };

            var response = await _client.AddGameAsync(request);
            return response.Response;
        }

        public async Task<string> DeleteGame(string userAsking, string game)
        {
            if (userAsking == null)
                return "Por favor ingrese su nombre de usuario para realizar este pedido";
            var response =  await _client.DeleteGameAsync(new DeleteGameRequest(){ User = userAsking, GameToDelete = game});
             return response.DeletedGame;
        }

        public async Task<string> ModifyGame(string gameToModify, GameModel model)
        {
            ModifyGameRequest request = new ModifyGameRequest()
            {
                Name = model.Name,
                Genre = model.Genre,
                Synopsis = model.Synopsis,
                OwnerUserName = model.OwnerUserName,
                PathToPhoto = model.PathToPhoto,
                GameToModify = gameToModify
            };
            var response = await _client.ModifyGameAsync(request);
            return response.Response;
        }
        
    }
}