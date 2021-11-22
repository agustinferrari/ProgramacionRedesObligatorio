using CommonProtocol.NetworkUtils;
using CommonProtocol.NetworkUtils.Interfaces;
using CommonProtocol.Protocol;
using System.Threading.Tasks;
using LogsModels;
using Server.BusinessLogic;
using Server.BusinessLogic.Interfaces;
using Server.Logic.Interfaces;

namespace Server.Logic.Commands.Strategies
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

        public abstract Task<LogGameModel> HandleRequest(Header header, INetworkStreamHandler clientNetworkStreamHandler);
    }
}
