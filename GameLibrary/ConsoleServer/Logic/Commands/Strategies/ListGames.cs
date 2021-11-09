using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using CommonLog;
using ConsoleServer.Utils.CustomExceptions;
using System.Threading.Tasks;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class ListGames : CommandStrategy
    {
        public override async Task<GameLogModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            GameLogModel log = new GameLogModel(header.ICommand);
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
