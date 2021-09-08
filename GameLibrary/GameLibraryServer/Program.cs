using System;

namespace GameLibraryServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1", 6000);
            server.startServer();
            Console.WriteLine("Hello World!");
        }
    }
}
