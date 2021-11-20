
using System.Threading.Tasks;
using Grpc.Core;
using ServerGRPC.BusinessLogic;
using ServerGRPC.Domain;


namespace ServerGRPC.Services
{
    public class UserService: UserProto.UserProtoBase
    {
        
        public override Task<UsersReply> GetUsers(UsersRequest request, ServerCallContext context)
        {
            // TODO Logear que este usuario hizo el request request.User
            UserController usersController = UserController.Instance;
            return Task.FromResult(new UsersReply
            {
                Users = "Usuarios en el sistema: " + usersController.GetAllUsers()
            });
        }
        
        public override Task<AddModifyUserReply> AddModifyUsers(AddModifyUserRequest userToAddRequest, ServerCallContext context)
        {
            // TODO Agregar log con userToAddRequest.User
            UserController usersController = UserController.Instance;
            usersController.TryAddUser(userToAddRequest.UserToAdd);
            
            return Task.FromResult(new AddModifyUserReply
            {
                Response = "El usuario " + userToAddRequest.UserToAdd + " fue creado correctamente"
            });
        }

        public override Task<DeleteUserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            UserController usersController = UserController.Instance;
            usersController.DeleteUser(request.UserToDelete);
            
            return Task.FromResult(new DeleteUserReply
            {
                DeletedUser = "El usuario " + request.UserToDelete + " fue borrado correctamente"
            });
        }
    }
}