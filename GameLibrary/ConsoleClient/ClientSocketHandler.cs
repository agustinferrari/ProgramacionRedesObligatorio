using Common.NetworkUtils;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Common.NetworkUtils.Interfaces;

namespace ConsoleClient
{
    public class ClientSocketHandler : SocketHandler
    {
        private readonly TcpClient _tcpClient;
        private static readonly ISettingsManager SettingsMgr = new SettingsManager();

        public ClientSocketHandler(IPEndPoint clientIpEndPoint) :
            base()
        {
            _tcpClient = new TcpClient(clientIpEndPoint);
            ConnectClient();
            _networkStream = _tcpClient.GetStream();
        }

        private async Task ConnectClient()
        {
            await _tcpClient.ConnectAsync(
                IPAddress.Parse(SettingsMgr.ReadSetting(ClientConfig.ServerIpConfigKey)),
                int.Parse(SettingsMgr.ReadSetting(ClientConfig.SeverPortConfigKey))).ConfigureAwait(false);
        }
    }
}
