using Common.NetworkUtils;
using System;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketHandler socketHandler = new SocketHandler("127.0.0.1", 6000);
            socketHandler.Listen();
            Console.WriteLine("Hello World!");
        }
    }
}
