using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class Login : MenuStrategy
    {
        public override string HandleSelectedOption(ISocketHandler clientSocket)
        {
            Console.WriteLine("Por favor ingrese el nombre de usuario para logearse: ");
            string user = Console.ReadLine().ToLower();
            string response = "";
            if (_menuValidator.ValidateNotEmptyFields(user))
            {
                clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.Login, user);
                Header header = clientSocket.ReceiveHeader();
                response = clientSocket.ReceiveString(header.IDataLength);
            }
            return response;
        }
    }
}
