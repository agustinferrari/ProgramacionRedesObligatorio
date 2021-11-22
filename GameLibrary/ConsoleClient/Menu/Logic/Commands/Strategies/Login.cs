using CommonProtocol.NetworkUtils.Interfaces;
using CommonProtocol.Protocol;
using System;
using System.Threading.Tasks;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class Login : MenuStrategy
    {
        public override async Task<string> HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            Console.WriteLine("Por favor ingrese el nombre de usuario para logearse: ");
            string user = Console.ReadLine().ToLower();
            string response = "";
            if (_menuValidator.ValidateNotEmptyFields(user))
            {
                await clientNetworkStream.SendMessage(HeaderConstants.Request, CommandConstants.Login, user);
                Header header = await clientNetworkStream.ReceiveHeader();
                response = await clientNetworkStream.ReceiveString(header.IDataLength);
            }
            return response;
        }
    }
}
