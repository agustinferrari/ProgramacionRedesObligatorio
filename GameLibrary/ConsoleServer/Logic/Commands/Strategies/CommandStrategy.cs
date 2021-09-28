using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleServer.BusinessLogic;
using ConsoleServer.BusinessLogic.Interfaces;
using ConsoleServer.Logic.Interfaces;

namespace ConsoleServer.Logic.Commands.Strategies
{
    public abstract class CommandStrategy
    {
        protected IClientHandler _clientHandler;
        protected IGameController _gameController;
        protected IUserController _userController;

        public CommandStrategy()
        {
            _clientHandler = ClientHandler.Instance;
            _gameController = GameController.Instance;
            _userController = UserController.Instance;
        }

        public abstract void HandleRequest(Header header, ISocketHandler clientSocketHandler);
    }
}
