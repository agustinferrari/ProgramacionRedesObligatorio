
using System.Threading.Tasks;
using Grpc.Core;
using ServerGRPC.BusinessLogic;


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
                Users = usersController.GetAllUsers()
            });
        }
    
    }
}