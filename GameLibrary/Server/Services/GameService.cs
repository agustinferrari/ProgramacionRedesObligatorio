using System;
using System.Threading.Tasks;
using Grpc.Core;
using Server.BusinessLogic;
using Server.Domain;
using Server.Utils.CustomExceptions;


namespace Server.Services
{
    public class GameService : GameProto.GameProtoBase
    {
        private readonly GameController _gamesController = GameController.Instance;
        private readonly UserController _usersController = UserController.Instance;


        public override Task<GamesReply> GetGames(GamesRequest request, ServerCallContext context)
        {
            // TODO Logear que este usuario hizo el request request.User
            string response;
            try
            {
                response = "Juegos en el sistema: \n" + _gamesController.GetAllGames();
                return Task.FromResult(new GamesReply
                {
                    Response = response
                });
            }
            catch (InvalidGameException e)
            {
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
        }

        public override Task<GamesReply> AddGame(AddGameRequest addGameModelRequest, ServerCallContext context)
        {
            string response;
            try
            {
                Game newGame = ParseGameModelToGame(addGameModelRequest);
                _gamesController.AddGame(newGame);
                response = "El juego " + addGameModelRequest.Name + " fue creado correctamente";
                return Task.FromResult(new GamesReply
                {
                    Response = response
                });
            }
            catch (InvalidUsernameException e)
            {
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
            catch (GameAlreadyAddedException e)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, e.Message));
            }
        }

        public override Task<GamesReply> ModifyGame(ModifyGameRequest modifyGameModelRequest, ServerCallContext context)
        {
            string response;
            try
            {
                Game newGame = ParseGameModelToGame(modifyGameModelRequest);
                Game oldGame = _gamesController.GetGame(modifyGameModelRequest.GameToModify);
                _gamesController.ModifyGame(oldGame, newGame);
                response = "El juego " + modifyGameModelRequest.Name + " fue modificado correctamente";
                return Task.FromResult(new GamesReply
                {
                    Response = response
                });
            }
            catch (Exception e) when (e is InvalidGameException || e is InvalidUsernameException)
            {
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
        }


        public override Task<GamesReply> DeleteGame(DeleteGameRequest deleteRequest, ServerCallContext context)
        {
            string response;
            try
            {
                Game game = _gamesController.GetGame(deleteRequest.GameToDelete);
                _gamesController.DeletePublishedGameByUser(game);
                response = "El juego " + deleteRequest.GameToDelete + " fue borrado correctamente";
                return Task.FromResult(new GamesReply
                {
                    Response = response
                });
            }
            catch (InvalidGameException e)
            {
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
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