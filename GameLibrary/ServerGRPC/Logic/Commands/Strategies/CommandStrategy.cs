using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using CommonLog;
using ServerGRPC.BusinessLogic;
using ServerGRPC.BusinessLogic.Interfaces;
using ServerGRPC.Logic.Interfaces;
using System.Threading.Tasks;

namespace ServerGRPC.Logic.Commands.Strategies
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

        public abstract Task<GameLogModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler);
    }
}
