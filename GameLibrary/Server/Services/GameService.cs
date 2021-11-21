using System.Threading.Tasks;
using Grpc.Core;
using Server.BusinessLogic;
using Server.Domain;
using Server.Utils.CustomExceptions;


namespace Server.Services
{
    public class GameService : GameProto.GameProtoBase
    {
        private readonly GameController _gamesController =  GameController.Instance;
        private readonly UserController _usersController =  UserController.Instance;

        
        public override Task<GamesReply> GetGames(GamesRequest request, ServerCallContext context)
        {
            // TODO Logear que este usuario hizo el request request.User
            string response;
            try
            {
                response = "Juegos en el sistema: \n" + _gamesController.GetAllGames();
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
                Game newGame = ParseGameModelToGame(addGameModelRequest);
                _gamesController.AddGame(newGame);
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
                Game newGame = ParseGameModelToGame(modifyGameModelRequest);
                Game oldGame = _gamesController.GetGame(modifyGameModelRequest.GameToModify);
                _gamesController.ModifyGame(oldGame,newGame);
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
                Game game = _gamesController.GetGame(deleteRequest.GameToDelete);
                _gamesController.DeletePublishedGameByUser(game);
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

        private Game ParseGameModelToGame(AddGameRequest model)
        {
            User user = _usersController.GetUser(model.OwnerUserName);
            return new Game
            {
              Name = model.Name,
              Genre = model.Genre,
              Synopsis = model.Synopsis,
              OwnerUser = user,
              PathToPhoto = model.PathToPhoto
            };
        }
        
        private Game ParseGameModelToGame(ModifyGameRequest model)
        {
            User user = _usersController.GetUser(model.OwnerUserName);
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