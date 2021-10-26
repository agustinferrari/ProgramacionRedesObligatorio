using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Utils.CustomExceptions;
using System.Threading.Tasks;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class ListFilteredGames : CommandStrategy
    {

        public override async Task HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            string rawData = await clientNetworkStreamHandler.ReceiveString(header.IDataLength)
                ;
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
        }
    }
}
