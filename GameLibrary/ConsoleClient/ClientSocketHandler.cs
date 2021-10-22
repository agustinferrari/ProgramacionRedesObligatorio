using Common.NetworkUtils;
using System.Net;
using System.Net.Sockets;

namespace ConsoleClient
{
    public class ClientSocketHandler : SocketHandler
    {
        private readonly TcpClient _tcpClient;

        public ClientSocketHandler(string ipAddress, int serverPort) :
            base()
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect(IPAddress.Parse(ipAddress), serverPort);
            _networkStream = _tcpClient.GetStream();
        }
    }
}
