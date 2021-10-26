using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class GetGameImage : CommandStrategy
    {
        public override async Task HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
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
        }
    }
}
