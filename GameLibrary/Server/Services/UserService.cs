
using System;
using System.Threading.Tasks;
using Grpc.Core;
using Server.BusinessLogic;
using Server.Utils.CustomExceptions;


namespace Server.Services
{
    public class UserService : UserProto.UserProtoBase
    {
        private readonly UserController _usersController = UserController.Instance;

        public override Task<UsersReply> GetUsers(UsersRequest request, ServerCallContext context)
        {
            string response;
            try
            {
                response = "Usuarios en el sistema: \n" + _usersController.GetAllUsers();
                return Task.FromResult(new UsersReply
                {
                    Response = response
                });
            }
            catch (InvalidUsernameException e)
            {
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
        }

        public override Task<UsersReply> AddUser(AddModifyUserRequest userToAddRequest, ServerCallContext context)
        {
            string response;
            try
            {
                _usersController.AddUser(userToAddRequest.UserToAddModify);
                response = "El usuario " + userToAddRequest.UserToAddModify + " fue creado correctamente";
                return Task.FromResult(new UsersReply
                {
                    Response = response
                });
            }
            catch (UserAlreadyAddedException e)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, e.Message));
            }
        }

        public override Task<UsersReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            string response;
            try
            {
                _usersController.DeleteUser(request.UserToDelete);

                response = "El usuario " + request.UserToDelete + " fue borrado correctamente";
                return Task.FromResult(new UsersReply
                {
                    Response = response
                });
            }
            catch (InvalidUsernameException e)
            {
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
        }

        public override Task<UsersReply> BuyGame(BuyDeleteGameRequest request, ServerCallContext context)
        {
            string response;
            try
            {
                _usersController.BuyGame(request.UserAsking, request.Game);
                string ownedGames = _usersController.ListOwnedGameByUser(request.UserAsking);
                response = "El usuario " + request.UserAsking + " ha adquirido " + request.Game +
                           " correctamente.\n" +
                           "Sus juegos son: \n"
                           + ownedGames;
                return Task.FromResult(new UsersReply
                {
                    Response = response
                });
            }
            catch (Exception e) when (e is InvalidGameException || e is InvalidUsernameException || e is InvalidGameException)
            {
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
            catch (GameAlreadyBoughtException e)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, e.Message));
            }
        }

        public override Task<UsersReply> DeleteGameForUser(BuyDeleteGameRequest request, ServerCallContext context)
        {
            string response;
            try
            {
                _usersController.DeleteGameForUser(request.UserAsking, request.Game);
                string ownedGames = _usersController.ListOwnedGameByUser(request.UserAsking);
                response = "El usuario " + request.UserAsking + " ha eliminado " + request.Game +
                           " correctamente de su biblioteca.\n" +
                           "Sus juegos son: \n"
                           + ownedGames;
                return Task.FromResult(new UsersReply
                {
                    Response = response
                });
            }
            catch (Exception e) when (e is InvalidDeleteGameForUserException || e is InvalidUsernameException || e is InvalidGameException)
            {
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }

        }

        public override Task<UsersReply> ModifyUser(AddModifyUserRequest request, ServerCallContext context)
        {
            string response;
            try
            {
                _usersController.ModifyUserName(request.UserAsking, request.UserToAddModify);
                response = "El usuario " + request.UserAsking + " ha sido modificado correctaente.\n" +
                           "Su nuevo nombre es: " + request.UserToAddModify;
                return Task.FromResult(new UsersReply
                {
                    Response = response
                });
            }
            catch (InvalidUsernameException e)
            {
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }

        }
    }
}