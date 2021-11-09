using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using CommonLog;
using ConsoleServer.Utils.CustomExceptions;
using System.Threading.Tasks;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class ListFilteredGames : CommandStrategy
    {

        public override async Task<GameLogModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            GameLogModel log = new GameLogModel(header.ICommand);
            string rawData = await clientNetworkStreamHandler.ReceiveString(header.IDataLength);
            string responseMessageResult;
            try
            {
                responseMessageResult = _gameController.GetGamesFiltered(rawData);

            }
            catch (InvalidGameException)
            {
                responseMessageResult = ResponseConstants.NoAvailableGames;
            }

            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListFilteredGames, responseMessageResult);
            return log;
        }
    }
}
