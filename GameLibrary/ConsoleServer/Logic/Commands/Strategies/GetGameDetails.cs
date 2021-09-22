using Common.NetworkUtils;
using Common.Protocol;
using ConsoleServer.Domain;
using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class GetGameDetails : CommandStrategy
    {

        public override void HandleRequest(Header header, SocketHandler clientSocketHandler)
        {
            string gameName = clientSocketHandler.ReceiveString(header.IDataLength);
            string responseMessageResult;
            try
            {
                Game game = _gameController.GetGame(gameName);
                responseMessageResult = game.ToString();
            }
            catch (InvalidGameException e)
            {
                responseMessageResult = ResponseConstants.InvalidGameError;
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.GetGameDetails, responseMessageResult);
        }
    }
}
