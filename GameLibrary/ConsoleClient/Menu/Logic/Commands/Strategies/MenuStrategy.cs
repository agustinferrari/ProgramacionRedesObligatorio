using Common.NetworkUtils.Interfaces;
using ConsoleClient.Menu.Utils;
using ConsoleClient.Menu.Utils.Interfaces;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public abstract class MenuStrategy
    {
        protected IMenuValidator _menuValidator;

        public MenuStrategy()
        {
            _menuValidator = new MenuValidator();
        }
        public abstract string HandleSelectedOption(ISocketHandler clientSocket);
    }
}
