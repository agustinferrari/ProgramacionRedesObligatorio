
using System.Threading.Tasks;
using Grpc.Core;
using ServerGRPC.BusinessLogic;
using ServerGRPC.Domain;
using ServerGRPC.Utils.CustomExceptions;


namespace ServerGRPC.Services
{
    public class UserService: UserProto.UserProtoBase
    {
        
        public override Task<UsersReply> GetUsers(UsersRequest request, ServerCallContext context)
        {
            // TODO Logear que este usuario hizo el request request.User
            string response;
            try
            {
                UserController usersController = UserController.Instance;
                response = "Usuarios en el sistema: \n" + usersController.GetAllUsers();
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
                UserController usersController = UserController.Instance;
                usersController.TryAddUser(userToAddRequest.UserToAddModify);
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
                UserController usersController = UserController.Instance;
                usersController.DeleteUser(request.UserToDelete);

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

        public override Task<BuyGameReply> BuyGame(BuyGameRequest request, ServerCallContext context)
        {
            UserController usersController = UserController.Instance;
            string response;
            try
            {
                usersController.BuyGame(request.UserAsking, request.GameToBuy);
                string ownedGames = usersController.ListOwnedGameByUser(request.UserAsking);
                response = "El usuario " + request.UserAsking + " ha adquirido " + request.GameToBuy +
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
            return Task.FromResult(new BuyGameReply
                {
                    Response = response
                });
        }

        public override Task<AddModifyUserReply> ModifyUser(AddModifyUserRequest request, ServerCallContext context)
        {
            string response;
            try
            {
                UserController usersController = UserController.Instance;
                usersController.ModifyUserName(request.UserAsking, request.UserToAddModify);
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