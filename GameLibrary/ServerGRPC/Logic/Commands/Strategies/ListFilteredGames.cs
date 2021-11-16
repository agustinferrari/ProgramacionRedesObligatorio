using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using CommonModels;
using ServerGRPC.Utils.CustomExceptions;
using System.Threading.Tasks;

namespace ServerGRPC.Logic.Commands.Strategies
{
    public class ListFilteredGames : CommandStrategy
    {

        public override async Task<GameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            GameModel log = new GameModel(header.ICommand);
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
