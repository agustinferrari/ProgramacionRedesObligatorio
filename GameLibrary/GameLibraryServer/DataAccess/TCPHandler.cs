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
        private string ipAddress;
        private int port;

        public TCPHandler(string ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void startConnection()
        {
            IPEndPoint clientIpEndPoint = new IPEndPoint(IPAddress.Parse(this.ipAddress), 0);
            tcpClient = new TcpClient(clientIpEndPoint);
            IPEndPoint serverIpEndPoint = new IPEndPoint(IPAddress.Parse(this.ipAddress), this.port);
            Console.WriteLine($"Trying to connect to server on IP {0} and port {1}", this.ipAddress, this.port);
            tcpClient.Connect(serverIpEndPoint);
        }

        public void closeConnection()
        {
            tcpClient.Close();
        }
    }
}
