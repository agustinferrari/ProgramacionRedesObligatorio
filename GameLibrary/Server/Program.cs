using System;
using System.Threading.Tasks;
using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Server.Presentation;

namespace Server
{
    public class Program
    {
        private static readonly ISettingsManager SettingsMgr = new SettingsManager();
        
        public static void Main(string[] args)
        {
            string serverIpAddress = SettingsMgr.ReadSetting(ServerConfig.ServerIpConfigKey);
            string serverPort = SettingsMgr.ReadSetting(ServerConfig.SeverPortConfigKey);
            IPortValidator validatorPort = new PortValidator();
            if (validatorPort.Validate(serverPort))
            {
                int parsedPort = Int32.Parse(serverPort);
                ServerNetworkStreamHandler serverNetworkStreamHandler = new ServerNetworkStreamHandler(serverIpAddress, parsedPort);

                serverNetworkStreamHandler.CreateClientConectionTask();

                ServerMenuRenderer.LoadMainMenu();
                Task.Run(() =>  ServerMenuHandler.HandleMainMenuResponse(serverNetworkStreamHandler));
            }
            else
            {
                Console.WriteLine("Por favor comprobar la configuracion del app config e ingrese un puerto valido");
            }
            CreateHostBuilder(args).Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}