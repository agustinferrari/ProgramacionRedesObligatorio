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
    public class GetGameDetails : CommandStrategy
    {

        public override void HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            string gameName = clientNetworkStreamHandler.ReceiveString(header.IDataLength).Result;
            string responseMessageResult;
            if (_clientHandler.IsSocketInUse(clientNetworkStreamHandler))
            {
                try
                {
                    Game game = _gameController.GetGame(gameName);
                    responseMessageResult = game.ToString();
                }
                catch (InvalidGameException)
                {
                    responseMessageResult = ResponseConstants.InvalidGameError;
                }
            }
            else
                responseMessageResult = ResponseConstants.AuthenticationError;
            clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.GetGameDetails, responseMessageResult);
        }
    }
}
