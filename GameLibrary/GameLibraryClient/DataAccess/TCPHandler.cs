using Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GameLibraryServer.DataAccess
{
    public class TCPHandler
    {
        public TcpClient tcpClient;
        private string _ipAddress;
        private int _port;

        public TCPHandler(string ipAddress, int port)
        {
            this._ipAddress = ipAddress;
            this._port = port;
        }

        public void startConnection()
        {
            IPEndPoint clientIpEndPoint = new IPEndPoint(IPAddress.Parse(this._ipAddress), 0);
            tcpClient = new TcpClient(clientIpEndPoint);
            IPEndPoint serverIpEndPoint = new IPEndPoint(IPAddress.Parse(this._ipAddress), this._port);
            Console.WriteLine($"Trying to connect to server on IP {0} and port {1}", this._ipAddress, this._port);
            tcpClient.Connect(serverIpEndPoint);

            using (var networkStream = tcpClient.GetStream())
            {
                while (true)
                {
                    var header = Console.ReadLine();
                    var word = Console.ReadLine();
                    byte[] headerData = Encoding.UTF8.GetBytes(header);
                    byte[] wordData = Encoding.UTF8.GetBytes(word);
                    byte[] wordDataLength = BitConverter.GetBytes(wordData.Length);
                    networkStream.Write(headerData, 0, Protocol.HeaderLength); // Enivamos Header
                    networkStream.Write(wordDataLength, 0, Protocol.WordLength); // Enivamos XXXX
                    networkStream.Write(wordData, 0, wordData.Length); // Enviamos DATA
                    if (word.Equals("exit"))
                    {
                        //keepConnection = false;
                    }
                }
            }
        }

        public void closeConnection()
        {
            tcpClient.Close();
        }
    }
}
