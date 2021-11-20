
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
        
        public override Task<AddModifyUserReply> AddModifyUsers(AddModifyUserRequest userToAdd, ServerCallContext context)
        {
            UserController usersController = UserController.Instance;
            usersController.TryAddUser(userToAdd.Name);
            
            return Task.FromResult(new AddModifyUserReply
            {
                Response = "El usuario " + userToAdd.Name + " fue creado correctamente"
            });
        }
    
    }
}