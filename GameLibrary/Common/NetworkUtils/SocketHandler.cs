using Common.Protocol;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common.NetworkUtils
{
    public class SocketHandler
    {
        protected Socket _socket;
        protected string _ipAddress;
        protected int _port;

        public SocketHandler(string ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(_ipAddress), _port));
        }

        public void Connect()
        {
            _socket.Connect(_ipAddress, _port);
        }

        public void Listen()
        {
            _socket.Listen(100);
        }

        public void SendMessage(string headerConstant, int commandNumber, string message)
        {
            SendHeader(headerConstant, commandNumber, message.Length);

            int sentBytes = 0;
            var bytesMessage = Encoding.UTF8.GetBytes(message);
            while (sentBytes < bytesMessage.Length)
            {
                sentBytes += _socket.Send(bytesMessage, sentBytes, bytesMessage.Length - sentBytes,
                    SocketFlags.None);
            }
        }

        public void SendHeader(string headerConstant, int commandNumber, int messageLength)
        {
            Header header = new Header(headerConstant, commandNumber, messageLength);
            byte[] data = header.GetRequest();
            int sentBytes = 0;
            while (sentBytes < data.Length)
            {
                sentBytes += _socket.Send(data, sentBytes, data.Length - sentBytes, SocketFlags.None);
            }
        }
    }
}
