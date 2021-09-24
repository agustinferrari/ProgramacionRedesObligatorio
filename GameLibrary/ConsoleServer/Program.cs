using Common.NetworkUtils;
using Common.NetworkUtils.Interface;
using ConsoleServer.BussinessLogic;
using ConsoleServer.Logic;
using ConsoleServer.Presentation;
using ConsoleServer.Utils;
using System;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleServer
{
    public class Program
    {
        private static readonly ISettingsManager SettingsMgr = new SettingsManager();
        static void Main(string[] args)
        {
            string serverIpAddress = SettingsMgr.ReadSetting(ServerConfig.ServerIpConfigKey);
            string serverPort = SettingsMgr.ReadSetting(ServerConfig.SeverPortConfigKey);
            if (PortValidator.Validate(serverPort))
            {
                int parsedPort = Int32.Parse(serverPort);
                ServerSocketHandler socketHandler = new ServerSocketHandler(serverIpAddress, parsedPort);
                socketHandler.CreateClientConectionThread();

                ServerMenuRenderer.LoadMainMenu();
                ServerMenuHandler.HandleMainMenuResponse(socketHandler);
            }
            else
            {
                Console.WriteLine("Please check your app config and enter a valid port");
            }
        }
    }
}
