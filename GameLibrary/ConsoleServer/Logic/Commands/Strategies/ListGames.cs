﻿using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.Utils.CustomExceptions;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public class ListGames : CommandStrategy
    {

        public override void HandleRequest(Header header, ISocketHandler clientSocketHandler)
        {
            string responseMessage;
            try
            {
                string gameList = _gameController.GetGames();
                responseMessage = gameList;
            }
            catch (InvalidGameException)
            {
                responseMessage = ResponseConstants.NoAvailableGames;
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListGames, responseMessage);
        }
    }
}
