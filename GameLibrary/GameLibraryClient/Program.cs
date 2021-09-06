using GameLibraryServer.DataAccess;
using System;

namespace GameLibraryServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPHandler tcpHandler = new TCPHandler("127.0.0.1", 6000);
            tcpHandler.startConnection();
        }
    }
}
