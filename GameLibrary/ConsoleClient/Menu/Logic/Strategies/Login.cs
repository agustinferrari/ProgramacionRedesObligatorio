using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Logic.Strategies
{
    public class Login : MenuStrategy
    {
        public override void HandleSelectedOption(SocketHandler clientSocket)
        {
            Console.WriteLine("Por favor ingrese el nombre de usuario para logearse: ");
            string user = Console.ReadLine().ToLower();
            if (_menuHandler.ValidateNotEmptyFields(user))
            {
                clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.Login, user);
                Header header = clientSocket.ReceiveHeader();
                string response = clientSocket.ReceiveString(header.IDataLength);
                Console.WriteLine(response);
                if (response == ResponseConstants.LoginSuccess)
                    _menuHandler.LoadLoggedUserMenu(clientSocket);
                else
                    _menuHandler.LoadMainMenu(clientSocket);
            }
            else
            {
                _menuHandler.LoadMainMenu(clientSocket);
            }
        }
    }
}
