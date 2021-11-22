
using System;
using System.Threading.Tasks;
using CommonProtocol.Protocol;
using Grpc.Core;
using LogsModels;
using Server.BusinessLogic;
using Server.Logic.LogManager;
using Server.Utils.CustomExceptions;


namespace Server.Services
{
    public class UserService : UserProto.UserProtoBase
    {
        private readonly UserController _usersController = UserController.Instance;
        private readonly LogLogic _logLogic = LogLogic.Instance;

        public override Task<UsersReply> GetUsers(UsersRequest request, ServerCallContext context)
        {
            LogGameModel log = new LogGameModel(CommandConstants.ModifyPublishedGame);
            log.User = request.UserAsking;
            string response;
            try
            {
                response = "Usuarios en el sistema: \n" + _usersController.GetAllUsers();
                log.Result = true;
                _logLogic.SendLog(log);
                return Task.FromResult(new UsersReply
                {
                    Response = response
                });
            }
            catch (InvalidUsernameException e)
            {
                log.Result = true;
                _logLogic.SendLog(log);
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
        }

        public override Task<UsersReply> AddUser(AddModifyUserRequest userToAddRequest, ServerCallContext context)
        {
            LogGameModel log = new LogGameModel(CommandConstants.AddUser);
            log.User = userToAddRequest.UserAsking;
            string response;
            try
            {
                _usersController.AddUser(userToAddRequest.UserToAddModify);
                response = "El usuario " + userToAddRequest.UserToAddModify + " fue creado correctamente";
                log.Result = true;
                _logLogic.SendLog(log);
                return Task.FromResult(new UsersReply
                {
                    Response = response
                });
            }
            catch (UserAlreadyAddedException e)
            {
                log.Result = true;
                _logLogic.SendLog(log);
                throw new RpcException(new Status(StatusCode.AlreadyExists, e.Message));
            }
        }

        public override Task<UsersReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            LogGameModel log = new LogGameModel(CommandConstants.DeleteUser);
            log.User = request.UserToDelete;
            string response;
            try
            {
                _usersController.DeleteUser(request.UserToDelete);

                response = "El usuario " + request.UserToDelete + " fue borrado correctamente";
                log.Result = true;
                _logLogic.SendLog(log);
                return Task.FromResult(new UsersReply
                {
                    Response = response
                });
            }
            catch (InvalidUsernameException e)
            {
                log.Result = true;
                _logLogic.SendLog(log);
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
        }

        public override Task<UsersReply> BuyGame(BuyDeleteGameRequest request, ServerCallContext context)
        {
            LogGameModel log = new LogGameModel(CommandConstants.BuyGame);
            log.User = request.UserAsking;
            log.Game = request.Game;
            string response;
            try
            {
                _usersController.BuyGame(request.UserAsking, request.Game);
                string ownedGames = _usersController.ListOwnedGameByUser(request.UserAsking);
                response = "El usuario " + request.UserAsking + " ha adquirido " + request.Game +
                           " correctamente.\n" +
                           "Sus juegos son: \n"
                           + ownedGames;
                log.Result = true;
                _logLogic.SendLog(log);
                return Task.FromResult(new UsersReply
                {
                    Response = response
                });
            }
            catch (Exception e) when (e is InvalidGameException || e is InvalidUsernameException || e is InvalidGameException)
            {
                log.Result = true;
                _logLogic.SendLog(log);
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
            catch (GameAlreadyBoughtException e)
            {
                log.Result = true;
                _logLogic.SendLog(log);
                throw new RpcException(new Status(StatusCode.AlreadyExists, e.Message));
            }
        }

        public override Task<UsersReply> DeleteGameForUser(BuyDeleteGameRequest request, ServerCallContext context)
        {
            LogGameModel log = new LogGameModel(CommandConstants.DeleteOwnedGame);
            log.User = request.UserAsking;
            string response;
            try
            {
                _usersController.DeleteGameForUser(request.UserAsking, request.Game);
                string ownedGames = _usersController.ListOwnedGameByUser(request.UserAsking);
                response = "El usuario " + request.UserAsking + " ha eliminado " + request.Game +
                           " correctamente de su biblioteca.\n" +
                           "Sus juegos son: \n"
                           + ownedGames;
                log.Result = true;
                _logLogic.SendLog(log);
                return Task.FromResult(new UsersReply
                {
                    Response = response
                });
            }
            catch (Exception e) when (e is InvalidDeleteGameForUserException || e is InvalidUsernameException || e is InvalidGameException)
            {
                log.Result = true;
                _logLogic.SendLog(log);
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }

        }

        public override Task<UsersReply> ModifyUser(AddModifyUserRequest request, ServerCallContext context)
        {
            LogGameModel log = new LogGameModel(CommandConstants.ModifyUser);
            log.User = request.UserAsking;
            string response;
            try
            {
                _usersController.ModifyUserName(request.UserAsking, request.UserToAddModify);
                response = "El usuario " + request.UserAsking + " ha sido modificado correctaente.\n" +
                           "Su nuevo nombre es: " + request.UserToAddModify;
                log.Result = true;
                _logLogic.SendLog(log);
                return Task.FromResult(new UsersReply
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

        }
    }
}