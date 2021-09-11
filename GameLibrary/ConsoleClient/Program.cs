using Common.NetworkUtils;
using ConsoleClient.Presentation;
using System;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientSocketHandler socketHandler = new ClientSocketHandler("127.0.0.1", 0, 6000);
            ClientMenuRenderer.LoadMainMenu();
            ClientMenuHandler.HandleMainMenuResponse(socketHandler);
            Console.WriteLine("Hello World!");
        }
    }
}
