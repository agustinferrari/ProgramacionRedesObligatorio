
using System.Threading.Tasks;
using Grpc.Core;
using ServerGRPC.BusinessLogic;
using ServerGRPC.Domain;
using ServerGRPC.Utils.CustomExceptions;


namespace ServerGRPC.Services
{
    public class UserService: UserProto.UserProtoBase
    {
        private readonly UserController _usersController = UserController.Instance;
        
        public override Task<UsersReply> GetUsers(UsersRequest request, ServerCallContext context)
        {
            // TODO Logear que este usuario hizo el request request.User
            string response;
            try
            {
                response = "Usuarios en el sistema: \n" + _usersController.GetAllUsers();
            }
            catch (InvalidUsernameException e)
            {
                response = e.Message;
            }
            return Task.FromResult(new UsersReply
            {
                Users = response
            });
        }
        
        public override Task<AddModifyUserReply> AddUser(AddModifyUserRequest userToAddRequest, ServerCallContext context)
        {
            // TODO Agregar log con userToAddRequest.User
            string response;
            try
            {
                _usersController.TryAddUser(userToAddRequest.UserToAddModify);
                response = "El usuario " + userToAddRequest.UserToAddModify + " fue creado correctamente";
            }
            catch (UserAlreadyAddedException e)
            {
                response = e.Message;
            }
            return Task.FromResult(new AddModifyUserReply
            {
                Response = response
            });
        }

        public override Task<DeleteUserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            string response;
            try
            {
                _usersController.DeleteUser(request.UserToDelete);

                response = "El usuario " + request.UserToDelete + " fue borrado correctamente";
            }
            catch (InvalidUsernameException e)
            {
                response = e.Message;
            }
            return Task.FromResult(new DeleteUserReply
            {
                DeletedUser = response
            });
        }

        public override Task<BuyDeleteGameReply> BuyGame(BuyDeleteGameRequest request, ServerCallContext context)
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
            }
            catch (InvalidGameException gameException)
            {
                response = gameException.Message;
            }
            catch (InvalidUsernameException usernameException)
            {
                response = usernameException.Message;
            }
            catch (GameAlreadyBoughtException gameAlreadyBoughtException)
            {
                response = gameAlreadyBoughtException.Message;
            }
            return Task.FromResult(new BuyDeleteGameReply
                {
                    Response = response
                });
        }

        public override Task<BuyDeleteGameReply> DeleteGameForUser(BuyDeleteGameRequest request, ServerCallContext context)
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
            }
            catch (InvalidDeleteGameForUserException gameException)
            {
                response = gameException.Message;
            }
            catch (InvalidUsernameException usernameException)
            {
                response = usernameException.Message;
            }
            catch (InvalidGameException e)
            {
                response = e.Message;
            }
            return Task.FromResult(new BuyDeleteGameReply
            {
                Response = response
            });
        }

        public override Task<AddModifyUserReply> ModifyUser(AddModifyUserRequest request, ServerCallContext context)
        {
            string response;
            try
            {
                _usersController.ModifyUserName(request.UserAsking, request.UserToAddModify);
                response = "El usuario " + request.UserAsking + " ha sido modificado correctaente.\n" +
                           "Su nuevo nombre es: " + request.UserToAddModify;
            }
            catch (InvalidUsernameException e)
            {
                response = e.Message;
            }
            return Task.FromResult(new AddModifyUserReply
            {
                Response = response
            });
        }
    }
}