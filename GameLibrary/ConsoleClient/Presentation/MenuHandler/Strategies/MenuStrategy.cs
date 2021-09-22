using Common.NetworkUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Presentation.MenuHandler.Strategies
{
    public abstract class MenuStrategy
    {
        protected ClientMenuHandler _menuHandler;

        public MenuStrategy()
        {
            _menuHandler = ClientMenuHandler.Instance;
        }
        public abstract void HandleSelectedOption(SocketHandler clientSocket);
    }
}
