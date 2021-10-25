using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class Login : MenuStrategy
    {
        public override string HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            Console.WriteLine("Por favor ingrese el nombre de usuario para logearse: ");
            string user = Console.ReadLine().ToLower();
            string response = "";
            if (_menuValidator.ValidateNotEmptyFields(user))
            {
                clientNetworkStream.SendMessage(HeaderConstants.Request, CommandConstants.Login, user);
                Header header = clientNetworkStream.ReceiveHeader().Result;
                response = clientNetworkStream.ReceiveString(header.IDataLength).Result;
            }
            return response;
        }
    }
}
