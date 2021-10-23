using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using ConsoleClient.Menu.Logic.Interfaces;
using ConsoleClient.Menu.Logic;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ConsoleClient
{
    public class Program
    {
        private static readonly ISettingsManager SettingsMgr = new SettingsManager();
        static async Task Main(string[] args)
        {
            try
            {
                IPEndPoint clientIpEndPoint = new IPEndPoint(
                    IPAddress.Parse(SettingsMgr.ReadSetting(ClientConfig.ClientIpConfigKey)),
                    int.Parse(SettingsMgr.ReadSetting(ClientConfig.ClientPortConfigKey)));
                ISocketHandler socketHandler = new ClientSocketHandler(clientIpEndPoint);
                IClientMenuHandler menuHandler = new ClientMenuHandler();
                menuHandler.LoadMainMenu(socketHandler);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Por favor comprobar la configuracion del app config e ingrese un puerto valido");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Por favor comprobar la configuracion del app config e ingrese un IP valida");
            }
            catch (IOException)
            {
                Console.WriteLine("Error al conectarse con el servidor.");
            }
        }
    }
}
