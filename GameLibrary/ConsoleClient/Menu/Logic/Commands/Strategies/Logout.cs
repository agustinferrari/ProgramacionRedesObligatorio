using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class Logout : MenuStrategy
    {
        public override string HandleSelectedOption(ISocketHandler clientSocket)
        {
            int sendNoData = 0;
            Header header = new Header(HeaderConstants.Request, CommandConstants.Logout, sendNoData);
            clientSocket.SendHeader(header);
            string response = clientSocket.RecieveResponse();
            return response;
            /*Console.WriteLine(response);
            if (response == ResponseConstants.LogoutSuccess)
                _menuHandler.LoadMainMenu(clientSocket);
            else
                _menuHandler.LoadLoggedUserMenu(clientSocket);*/
        }
    }
}
