using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Strategies
{
    public class Logout : MenuStrategy
    {
        public override void HandleSelectedOption(SocketHandler clientSocket)
        {
            int sendNoData = 0;
            Header header = new Header(HeaderConstants.Request, CommandConstants.Logout, sendNoData);
            clientSocket.SendHeader(header);
            string response = clientSocket.RecieveResponse();
            Console.WriteLine(response);
            if (response == ResponseConstants.LogoutSuccess)
                _menuHandler.LoadMainMenu(clientSocket);
            else
                _menuHandler.LoadLoggedUserMenu(clientSocket);
        }
    }
}
