using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class GetGameImage : CommandStrategy
    {
        public override void HandleRequest(Header header, ISocketHandler clientSocketHandler)
        {
            string gameName = clientSocketHandler.ReceiveString(header.IDataLength);
            string responseMessageResult = "";
            Game game = null;
            try
            {
                game = _gameController.GetGame(gameName);
            }
            catch (InvalidGameException)
            {
                responseMessageResult = ResponseConstants.InvalidGameError;
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.GetGameImage, responseMessageResult);
            if (game != null)
                clientSocketHandler.SendImage(game.PathToPhoto);
        }
    }
}
