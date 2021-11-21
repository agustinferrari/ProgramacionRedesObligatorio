using System.Threading.Tasks;
using CommonModels;

namespace ServerAdmin.ServicesGrpcInterfaces
{
    public interface IGameGrpc
    {

        public Task<string> GetGames(string userAsking);

        public Task<string> AddGame(GameModel model);

        public Task<string> DeleteGame(string userAsking, string game);

        public Task<string> ModifyGame(string gameToModify, GameModel model);
        
    }
}