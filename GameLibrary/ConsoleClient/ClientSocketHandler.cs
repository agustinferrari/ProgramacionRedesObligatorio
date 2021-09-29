using Common.NetworkUtils;

namespace ConsoleClient
{
    public class ClientSocketHandler : SocketHandler
    {
        public ClientSocketHandler(string ipAddress, int serverPort) :
            base()
        {
            _socket.Connect(ipAddress, serverPort);
        }
    }
}
