using Common.NetworkUtils;
using ConsoleClient.Presentation;
using System;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketHandler socketHandler = new SocketHandler("127.0.0.1", 0);
            socketHandler.Connect();
            ClientMenuRenderer.LoadMainMenu();
            ClientMenuHandler.handleMainMenuResponse();
            Console.WriteLine("Hello World!");
        }
    }
}
