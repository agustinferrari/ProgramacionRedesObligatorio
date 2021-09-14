using Common.NetworkUtils;
using ConsoleServer.Presentation;
using System;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerSocketHandler socketHandler = new ServerSocketHandler("127.0.0.1", 6000);
            socketHandler.CreateClientConectionThread();

            ServerMenuRenderer.LoadMainMenu();
            ServerMenuHandler.HandleMainMenuResponse(socketHandler);
        }
    }
}
