using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using ConsoleClient.Menu.Logic.Interfaces;
using ConsoleClient.Menu.Logic;
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
                ISocketHandler socketHandler = new ClientSocketHandler(serverIpAddress, parsedPort);
                IClientMenuHandler menuHandler = new ClientMenuHandler();
                menuHandler.LoadMainMenu(socketHandler);
            }
            else
            {
                Console.WriteLine("Por favor comprobar la configuracion del app config e ingrese un puerto valido");
            }
        }
    }
}
