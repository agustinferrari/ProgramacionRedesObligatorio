using CommonProtocol.NetworkUtils;
using CommonProtocol.NetworkUtils.Interfaces;
using CommonProtocol.Protocol;
using System.Threading.Tasks;
using LogsModels;
using Server.Utils.CustomExceptions;

namespace Server.Logic.Commands.Strategies
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
