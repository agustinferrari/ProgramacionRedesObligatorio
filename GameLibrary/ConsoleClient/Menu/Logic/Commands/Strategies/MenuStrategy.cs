using CommonProtocol.NetworkUtils.Interfaces;
using ConsoleClient.Menu.Utils;
using ConsoleClient.Menu.Utils.Interfaces;
using System.Threading.Tasks;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public abstract class MenuStrategy
    {
        protected IMenuValidator _menuValidator;

        public MenuStrategy()
        {
            _menuValidator = new MenuValidator();
        }
        public abstract Task<string> HandleSelectedOption(INetworkStreamHandler clientNetworkStream);
    }
}
