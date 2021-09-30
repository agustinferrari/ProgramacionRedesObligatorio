using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using ConsoleClient.Menu.Logic.Interfaces;
using ConsoleClient.Menu.Logic;
using System;
using System.Net.Sockets;

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
                try
                {
                    ISocketHandler socketHandler = new ClientSocketHandler(serverIpAddress, parsedPort);
                    IClientMenuHandler menuHandler = new ClientMenuHandler();
                    menuHandler.LoadMainMenu(socketHandler);
                }
                catch (SocketException)
                {
                    Console.WriteLine("Error al conectarse con el servidor.");
                }
            }
            else
            {
                Console.WriteLine("Por favor comprobar la configuracion del app config e ingrese un puerto valido");
            }
        }
    }
}
