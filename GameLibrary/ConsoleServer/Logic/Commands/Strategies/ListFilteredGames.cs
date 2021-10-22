using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Utils.CustomExceptions;


namespace ConsoleServer.Logic.Commands.Strategies
{
    public class ListFilteredGames : CommandStrategy
    {

        public override void HandleRequest(Header header, ISocketHandler clientSocketHandler)
        {
            string rawData = clientSocketHandler.ReceiveString(header.IDataLength).Result;
            string responseMessageResult;
            try
            {
             responseMessageResult = _gameController.GetGamesFiltered(rawData);

            }
            catch (InvalidGameException)
            {
                responseMessageResult = ResponseConstants.NoAvailableGames;
            }

            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListFilteredGames, responseMessageResult);
        }
    }
}
