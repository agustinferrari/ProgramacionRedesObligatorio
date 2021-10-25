using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using ConsoleServer.Presentation;
using System;
using System.Threading.Tasks;

namespace ConsoleServer
{
    public class Program
    {
        private static readonly ISettingsManager SettingsMgr = new SettingsManager();
        static async Task Main(string[] args)
        {
            string serverIpAddress = SettingsMgr.ReadSetting(ServerConfig.ServerIpConfigKey);
            string serverPort = SettingsMgr.ReadSetting(ServerConfig.SeverPortConfigKey);
            IPortValidator validatorPort = new PortValidator();
            if (validatorPort.Validate(serverPort))
            {
                int parsedPort = Int32.Parse(serverPort);
                ServerSocketHandler socketHandler = new ServerSocketHandler(serverIpAddress, parsedPort);

                await socketHandler.CreateClientConectionTask();

                ServerMenuRenderer.LoadMainMenu();
                ServerMenuHandler.HandleMainMenuResponse(socketHandler);
            }
            else
            {
                Console.WriteLine("Por favor comprobar la configuracion del app config e ingrese un puerto valido");
            }
        }
    }
}
