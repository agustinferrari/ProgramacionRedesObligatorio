using CommonProtocol.NetworkUtils;
using CommonProtocol.NetworkUtils.Interfaces;
using CommonProtocol.Protocol;
using System.Threading.Tasks;
using LogsModels;
using Server.Domain;
using Server.Utils.CustomExceptions;

namespace Server.Logic.Commands.Strategies
{
    public class GetGameImage : CommandStrategy
    {
        public override async Task<LogGameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            LogGameModel log = new LogGameModel(header.ICommand);
            string gameName = await clientNetworkStreamHandler.ReceiveString(header.IDataLength);
            string responseMessageResult = "";
            Game game = null;
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
            {

                try
                {
                    game = _gameController.GetGame(gameName);
                }
                catch (InvalidGameException)
                {
                    responseMessageResult = ResponseConstants.InvalidGameError;
                }
            }
            else
                responseMessageResult = ResponseConstants.AuthenticationError;
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.GetGameImage, responseMessageResult);
            if (game != null)
                await clientNetworkStreamHandler.SendImage(game.PathToPhoto);
            return log;
        }
    }
}
