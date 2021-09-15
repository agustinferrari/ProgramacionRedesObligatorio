using Common.NetworkUtils;
using ConsoleServer.BussinessLogic;
using ConsoleServer.Logic;
using ConsoleServer.Presentation;
using ConsoleServer.Utils;
using System;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientHandler.gameController = new GameController();
            ClientHandler.userController = new UserController();
            CatalogueLoader.AddGames(ClientHandler.gameController);
            ServerSocketHandler socketHandler = new ServerSocketHandler("127.0.0.1", 6000);
            socketHandler.CreateClientConectionThread();

            ServerMenuRenderer.LoadMainMenu();
            ServerMenuHandler.HandleMainMenuResponse(socketHandler);
        }
    }
}
