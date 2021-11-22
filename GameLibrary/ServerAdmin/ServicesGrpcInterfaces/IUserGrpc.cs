using System.Threading.Tasks;
using CommonModels;

namespace ServerAdmin.ServicesGrpcInterfaces
{
    public interface IUserGrpc
    {
        public Task<string> GetUsers(string userAsking);
        public Task<string> AddUser(string userAsking, UserModel userToAdd);
        public Task<string> DeleteUser(string userAsking, string userToDelete);
        public Task<string> BuyGame(string userAsking, string gameToBuy);
        public Task<string> ModifyUser(string userAsking, UserModel newUserName);
        public Task<string> DeleteGameForUser(string userAsking, string gameToDeleteForUser);
    }
}