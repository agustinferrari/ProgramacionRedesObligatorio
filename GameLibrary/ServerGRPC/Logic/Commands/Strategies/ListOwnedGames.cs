using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System.Threading.Tasks;
using LogsModels;

namespace ServerGRPC.Logic.Commands.Strategies
{
    public class ListOwnedGames : CommandStrategy
    {

        public override async Task<LogGameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            LogGameModel log = new LogGameModel(header.ICommand);
            string emptyString = "";
            string responseMessage;
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
            {
                string userName = _clientHandler.GetUsername(clientNetworkStreamHandler);
                string gameList = _userController.ListOwnedGameByUser(userName);
                responseMessage = gameList;
                if (gameList == emptyString)
                    responseMessage = ResponseConstants.LibraryError;

            }
            else
                responseMessage = ResponseConstants.AuthenticationError;
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListOwnedGames, responseMessage);
            return log;
        }
    }
}
