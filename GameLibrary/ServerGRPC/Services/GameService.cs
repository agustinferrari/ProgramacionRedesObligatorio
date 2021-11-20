using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ServerGRPC.BusinessLogic;
using ServerGRPC.Domain;
using ServerGRPC.Utils.CustomExceptions;


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
            string response;
            try
            {
                GameController gamesController = GameController.Instance;
                response = "Juegos en el sistema: \n" + gamesController.GetAllGames();
            }
            catch (InvalidGameException e)
            {
                response = e.Message;
            }
            return Task.FromResult(new GamesReply
            {
                Games = response
            });
        }

        public override Task<AddGameReply> AddGame(AddGameRequest addGameModelRequest, ServerCallContext context)
        {
            string response;
            try
            {
                GameController gamesController = GameController.Instance;
                Game newGame = parseGameModelToGame(addGameModelRequest);
                gamesController.AddGame(newGame);
                response = "El juego " + addGameModelRequest.Name + " fue creado correctamente";
            }
            catch (InvalidUsernameException e)
            {
                response = e.Message;
            }
            catch (GameAlreadyAddedException e)
            {
                response = e.Message;
            }
            return Task.FromResult(new AddGameReply
            {
                Response = response
            });
        }
        
        public override Task<ModifyGameReply> ModifyGame(ModifyGameRequest modifyGameModelRequest, ServerCallContext context)
        {
            string response;
            try
            {
                GameController gamesController = GameController.Instance;
                Game newGame = parseGameModelToGame(modifyGameModelRequest);
                Game oldGame = gamesController.GetGame(modifyGameModelRequest.GameToModify);
                gamesController.ModifyGame(oldGame,newGame);
                response ="El juego " + modifyGameModelRequest.Name + " fue modificado correctamente";
            }
            catch (InvalidUsernameException e)
            {
                response = e.Message;
            }
            catch (InvalidGameException e)
            {
                response = e.Message;
            }
            return Task.FromResult(new ModifyGameReply
            {
                Response = response
            });
        }
       

        public override Task<DeleteGameReply> DeleteGame(DeleteGameRequest deleteRequest, ServerCallContext context)
        {
            string response;
            try
            {
                GameController gamesController = GameController.Instance;
                Game game = gamesController.GetGame(deleteRequest.GameToDelete);
                gamesController.DeletePublishedGameByUser(game);
                response = "El juego " + deleteRequest.GameToDelete + " fue borrado correctamente";
            }
            catch (InvalidGameException e)
            {
                response = e.Message;
            }
            return Task.FromResult(new DeleteGameReply
            {
                DeletedGame = response
            });
        }

        private Game parseGameModelToGame(AddGameRequest model)
        {
            User user = UserController.Instance.GetUser(model.OwnerUserName);
            return new Game
            {
              Name = model.Name,
              Genre = model.Genre,
              Synopsis = model.Synopsis,
              OwnerUser = user,
              PathToPhoto = model.PathToPhoto
            };
        }
        
        private Game parseGameModelToGame(ModifyGameRequest model)
        {
            User user = UserController.Instance.GetUser(model.OwnerUserName);
            return new Game
            {
                Name = model.Name,
                Genre = model.Genre,
                Synopsis = model.Synopsis,
                OwnerUser = user,
                PathToPhoto = model.PathToPhoto
            };
        }
    
    }
}