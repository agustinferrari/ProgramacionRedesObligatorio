using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using ConsoleClient.Menu.Logic.Interfaces;
using ConsoleClient.Menu.MenuHandler;
using System;

namespace ConsoleClient
{
    public class Program
    {
        private static readonly ISettingsManager SettingsMgr = new SettingsManager();
        static void Main(string[] args)
        {
            string serverIpAddress = SettingsMgr.ReadSetting(ClientConfig.ClientIpConfigKey);
            string serverPort = SettingsMgr.ReadSetting(ClientConfig.ClientPortConfigKey);
            IPortValidator validatorPort = new PortValidator();
            if (validatorPort.Validate(serverPort))
            {
                int parsedPort = Int32.Parse(serverPort);
                ClientSocketHandler socketHandler = new ClientSocketHandler(serverIpAddress, 0, parsedPort);
                IClientMenuHandler menuHandler = ClientMenuHandler.Instance;
                menuHandler.LoadMainMenu(socketHandler);
            }
            else
            {
                Console.WriteLine("Please check your app config and enter a valid port");
            }
        }
    }
}
