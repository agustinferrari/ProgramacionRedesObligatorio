using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class Logout : MenuStrategy
    {
        public override void HandleSelectedOption(ISocketHandler clientSocket)
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
