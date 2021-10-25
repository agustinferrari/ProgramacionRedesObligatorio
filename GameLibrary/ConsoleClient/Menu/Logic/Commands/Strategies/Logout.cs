using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;

namespace ConsoleClient.Menu.Logic.Commands.Strategies
{
    public class Logout : MenuStrategy
    {
        public override string HandleSelectedOption(INetworkStreamHandler clientNetworkStream)
        {
            int sendNoData = 0;
            Header header = new Header(HeaderConstants.Request, CommandConstants.Logout, sendNoData);
            clientNetworkStream.SendHeader(header);
            string response = clientNetworkStream.RecieveResponse().Result;
            return response;
        }
    }
}
