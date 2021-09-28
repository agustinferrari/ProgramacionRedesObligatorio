using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using ConsoleClient.Menu.Logic.Interfaces;
using ConsoleClient.Menu.MenuHandler;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public abstract class MenuStrategy
    {
        protected IClientMenuHandler _menuHandler;

        public MenuStrategy()
        {
            _menuHandler = new ClientMenuHandler();
        }
        public abstract string HandleSelectedOption(ISocketHandler clientSocket);
    }
}
