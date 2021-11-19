﻿using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ServerGRPC.Domain;
using ServerGRPC.Utils.CustomExceptions;
using System.Threading.Tasks;
using LogsModels;

namespace ServerGRPC.Logic.Commands.Strategies
{
    public class GetGameDetails : CommandStrategy
    {

        public override async Task<LogGameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler)
        {
            LogGameModel log = new LogGameModel(header.ICommand);
            string gameName = await clientNetworkStreamHandler.ReceiveString(header.IDataLength);
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
            await clientNetworkStreamHandler.SendMessage(HeaderConstants.Response, CommandConstants.GetGameDetails, responseMessageResult);
            return log;
        }
    }
}
