using Common.NetworkUtils;
using System;
using System.Net.Sockets;

namespace ConsoleClient
{
    public class ClientSocketHandler : SocketHandler
    {
        public ClientSocketHandler(string ipAddress, int port, int serverPort) :
            base(ipAddress, port)
        {
            _socket.Connect(_ipAddress, serverPort);
        }


    }
}
