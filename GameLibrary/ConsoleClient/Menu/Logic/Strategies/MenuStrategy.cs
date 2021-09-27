using Common.NetworkUtils;
using ConsoleClient.Menu.Logic.Interfaces;
using ConsoleClient.Menu.MenuHandler;

namespace ConsoleClient.Menu.Logic.Strategies
{
    public abstract class MenuStrategy
    {
        protected IClientMenuHandler _menuHandler;

        public MenuStrategy()
        {
            _menuHandler = ClientMenuHandler.Instance;
        }
        public abstract void HandleSelectedOption(SocketHandler clientSocket);
    }
}
