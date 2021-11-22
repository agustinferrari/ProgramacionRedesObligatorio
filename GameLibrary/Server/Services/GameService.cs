using System;
using System.Threading.Tasks;
using CommonProtocol.Protocol;
using Grpc.Core;
using LogsModels;
using Server.BusinessLogic;
using Server.Domain;
using Server.Logic.LogManager;
using Server.Utils.CustomExceptions;


namespace Server.Services
{
    public class GameService : GameProto.GameProtoBase
    {
        private readonly GameController _gamesController = GameController.Instance;
        private readonly UserController _usersController = UserController.Instance;
        private readonly LogLogic _logLogic = LogLogic.Instance;


        public override Task<GamesReply> GetGames(GamesRequest request, ServerCallContext context)
        {
            LogGameModel log = new LogGameModel(CommandConstants.ListGames);
            log.User = request.User;
            string response;
            try
            {
                response = "Juegos en el sistema: \n" + _gamesController.GetAllGames();
                log.Result = true;
                _logLogic.SendLog(log);
                return Task.FromResult(new GamesReply
                {
                    Response = response
                });
            }
            catch (InvalidGameException e)
            {
                log.Result = false;
                _logLogic.SendLog(log);
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
        }

        public override Task<GamesReply> AddGame(AddGameRequest addGameModelRequest, ServerCallContext context)
        {
            LogGameModel log = new LogGameModel(CommandConstants.AddGame);
            log.User = addGameModelRequest.OwnerUserName;
            log.Game = addGameModelRequest.Name;
            string response;
            try
            {
                Game newGame = ParseGameModelToGame(addGameModelRequest);
                _gamesController.AddGame(newGame);
                response = "El juego " + addGameModelRequest.Name + " fue creado correctamente";
                log.Result = true;
                _logLogic.SendLog(log);
                return Task.FromResult(new GamesReply
                {
                    Response = response
                });
            }
            catch (InvalidUsernameException e)
            {
                log.Result = false;
                _logLogic.SendLog(log);
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
            catch (GameAlreadyAddedException e)
            {
                log.Result = false;
                _logLogic.SendLog(log);
                throw new RpcException(new Status(StatusCode.AlreadyExists, e.Message));
            }
        }

        public override Task<GamesReply> ModifyGame(ModifyGameRequest modifyGameModelRequest, ServerCallContext context)
        {
            LogGameModel log = new LogGameModel(CommandConstants.ModifyPublishedGame);
            log.User = modifyGameModelRequest.OwnerUserName;
            log.Game = modifyGameModelRequest.Name;
            string response;
            try
            {
                Game newGame = ParseGameModelToGame(modifyGameModelRequest);
                Game oldGame = _gamesController.GetGame(modifyGameModelRequest.GameToModify);
                _gamesController.ModifyGame(oldGame, newGame);
                response = "El juego " + modifyGameModelRequest.Name + " fue modificado correctamente";
                log.Result = true;
                _logLogic.SendLog(log);
                return Task.FromResult(new GamesReply
                {
                    Response = response
                });
            }
            catch (Exception e) when (e is InvalidGameException || e is InvalidUsernameException)
            {
                log.Result = false;
                _logLogic.SendLog(log);
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
        }


        public override Task<GamesReply> DeleteGame(DeleteGameRequest deleteRequest, ServerCallContext context)
        {
            LogGameModel log = new LogGameModel(CommandConstants.DeletePublishedGame);
            log.User = deleteRequest.User;
            log.Game = deleteRequest.GameToDelete;
            string response;
            try
            {
                Game game = _gamesController.GetGame(deleteRequest.GameToDelete);
                _gamesController.DeletePublishedGameByUser(game);
                response = "El juego " + deleteRequest.GameToDelete + " fue borrado correctamente";
                log.Result = true;
                _logLogic.SendLog(log);
                return Task.FromResult(new GamesReply
                {
                    Response = response
                });
            }
            catch (InvalidGameException e)
            {
                log.Result = false;
                _logLogic.SendLog(log);
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