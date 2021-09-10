using System;
using System.Collections.Generic;
using System.Text;

namespace Common.NetworkUtils
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
