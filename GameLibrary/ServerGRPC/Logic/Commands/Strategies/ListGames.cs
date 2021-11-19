﻿using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ServerGRPC.Utils.CustomExceptions;
using System.Threading.Tasks;
using LogsModels;

namespace ServerGRPC.Logic.Commands.Strategies
{
    public class ListGames : CommandStrategy
    {
        public override async Task<LogGameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            LogGameModel log = new LogGameModel(header.ICommand);
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
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.ListGames, responseMessage);
            return log;
        }
    }
}
