using Common.Protocol;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common.NetworkUtils
{
    public static class SocketHandler
    {
        private static Socket _socket;
        private static void createSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0));
            _socket.Connect("127.0.0.1", 20000);
        }

        public static void sendMessage(string headerConstant, int commandNumber, string message)
        {
            sendHeader(headerConstant, commandNumber, message.Length);

            int sentBytes = 0;
            var bytesMessage = Encoding.UTF8.GetBytes(message);
            while (sentBytes < bytesMessage.Length)
            {
                sentBytes += _socket.Send(bytesMessage, sentBytes, bytesMessage.Length - sentBytes,
                    SocketFlags.None);
            }
        }

        public static void sendHeader(string headerConstant, int commandNumber, int messageLength)
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
