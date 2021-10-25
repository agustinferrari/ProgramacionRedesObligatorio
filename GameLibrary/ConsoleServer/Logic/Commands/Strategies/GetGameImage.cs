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
        public override void HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            string gameName = clientNetworkStreamHandler.ReceiveString(header.IDataLength).Result;
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
            clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.GetGameImage, responseMessageResult);
            if (game != null)
                clientNetworkStreamHandler.SendImage(game.PathToPhoto);
        }
    }
}
