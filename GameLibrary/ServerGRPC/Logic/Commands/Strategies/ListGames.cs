using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using CommonModels;
using ServerGRPC.Utils.CustomExceptions;
using System.Threading.Tasks;

namespace ServerGRPC.Logic.Commands.Strategies
{
    public class ListGames : CommandStrategy
    {
        public override async Task<GameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            GameModel log = new GameModel(header.ICommand);
            string responseMessage;
            try
            {
                string gameList = _gameController.GetGames();
                responseMessage = gameList;
            }
            catch (InvalidGameException)
            {
                responseMessage = ResponseConstants.NoAvailableGames;
            }
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListGames, responseMessage);
            return log;
        }
    }
}
