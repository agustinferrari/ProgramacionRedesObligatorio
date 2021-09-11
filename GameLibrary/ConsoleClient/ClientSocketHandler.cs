using Common.NetworkUtils;


namespace ConsoleClient
{
    public class ClientSocketHandler : SocketHandler
    {
        public ClientSocketHandler(string ipAddress, int port, int serverPort) :
            base(ipAddress, port)
        {
            _socket.Connect(_ipAddress, serverPort);
        }
    }
}
