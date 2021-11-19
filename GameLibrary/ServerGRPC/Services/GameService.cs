
using System;
using System.Threading.Tasks;
using CommonModels;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ServerGRPC.BusinessLogic;
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
            // TODO Logear que este usuario hizo el request request.User
            GameController gamesController = GameController.Instance;
            return Task.FromResult(new GamesReply
            {
                Games = gamesController.GetGames()
            });
        }
        
        public override Task<AddUpdateGameReply> AddModifyGames(AddUpdateGameRequest addGameModel, ServerCallContext context)
        {
            GameController gamesController = GameController.Instance;
            Game newGame = parseGameModelToGame(addGameModel);
            gamesController.AddGame(newGame);
            
            return Task.FromResult(new AddUpdateGameReply
            {
                Response = "El juego " + addGameModel.Name + " fue creado correctamente"
            });
        }

        public override Task<DeleteGameReply> DeleteGame(DeleteGameRequest deleteRequest, ServerCallContext context)
        {
            GameController gamesController = GameController.Instance;
            Game game = gamesController.GetGame(deleteRequest.GameToDelete);
            gamesController.DeletePublishedGameByUser(game);
            return Task.FromResult(new DeleteGameReply
            {
                 DeletedGame= "El juego " + deleteRequest.GameToDelete + " fue borrado correctamente"
            });
        }

        private Game parseGameModelToGame(AddUpdateGameRequest model)
        {
            User user = UserController.Instance.GetUser(model.OwnerUserName);
            return new Game
            {
              Name = model.Name,
              Genre = model.Genre,
              Rating = model.Rating,
              Synopsis = model.Synopsis,
              OwnerUser = user,
              PathToPhoto = model.PathToPhoto
            };
        }
    
    }
}