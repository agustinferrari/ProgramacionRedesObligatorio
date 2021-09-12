using Common.Protocol;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common.NetworkUtils
{
    public class SocketHandler
    {
        public Socket _socket;
        protected string _ipAddress;
        protected int _port;

        public SocketHandler(Socket socket)
        {
            _socket = socket;
        }

        public SocketHandler(string ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(_ipAddress), _port));
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

        public void ReceiveData(int Length, byte[] buffer)
        {
            var iRecv = 0;
            while (iRecv < Length)
            {
                try
                {
                    var localRecv = _socket.Receive(buffer, iRecv, Length - iRecv, SocketFlags.None);
                    if (localRecv == 0) // Si recieve retorna 0 -> la conexion se cerro desde el endpoint remoto
                    {
                        throw new Exception("No data received"); // Catchear en server y si el server sigue andando cerrar la conexion 
                    }
                    iRecv += localRecv;
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.Message);
                    return;
                }
            }
        }

        public Header ReceiveHeader()
        {
            int headerLength = HeaderConstants.Request.Length + HeaderConstants.CommandLength +
                                   HeaderConstants.DataLength;
            byte[] buffer = new byte[headerLength];
            this.ReceiveData(headerLength, buffer);
            Header header = new Header();
            header.DecodeData(buffer);
            return header;
        }

        public string ReceiveString(int dataLength)
        {
            byte[] bufferData = new byte[dataLength];
            this.ReceiveData(dataLength, bufferData);
            string data = Encoding.UTF8.GetString(bufferData);
            return data;
        }

        public void ShutdownSocket()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
    }
}
