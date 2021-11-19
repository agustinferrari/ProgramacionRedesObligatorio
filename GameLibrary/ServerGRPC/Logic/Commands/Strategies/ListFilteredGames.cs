using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ServerGRPC.Utils.CustomExceptions;
using System.Threading.Tasks;
using LogsModels;

namespace ServerGRPC.Logic.Commands.Strategies
{
    public class ListFilteredGames : CommandStrategy
    {

        public override async Task<LogGameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            LogGameModel log = new LogGameModel(header.ICommand);
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
