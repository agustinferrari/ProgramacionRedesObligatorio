using System;
using System.Collections.Generic;
using System.Text;

namespace Common.NetworkUtils
{
    public class ServerSocketHandler : SocketHandler
    {
        public ServerSocketHandler(string ipAddress, int port) :
            base(ipAddress, port)
        {
            _socket.Listen(100);
        }
    }
}
