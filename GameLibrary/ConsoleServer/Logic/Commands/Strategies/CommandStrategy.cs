using Common.NetworkUtils;
using Common.Protocol;
using ConsoleServer.BussinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public abstract class CommandStrategy
    {
        protected ClientHandler _clientHandler;
        protected GameController _gameController;
        protected UserController _userController;

        public CommandStrategy()
        {
            _clientHandler = ClientHandler.Instance;
            _gameController = GameController.Instance;
            _userController = UserController.Instance;
        }

        public abstract void HandleRequest(Header header, SocketHandler clientSocketHandler);
    }
}
