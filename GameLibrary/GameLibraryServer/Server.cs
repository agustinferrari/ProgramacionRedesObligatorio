using Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GameLibraryServer
{
    public class Server
    {
        private string _ipAddress;
        private int _port;

        public Server(string ipAddress, int port)
        {
            this._ipAddress = ipAddress;
            this._port = port;
        }

        public void startServer()
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(this._ipAddress), this._port);
            var tcpListener = new TcpListener(ipEndPoint);
            tcpListener.Start(100);
            Console.WriteLine("Server will start displaying messages from the clients");

            while (true)
            {
                var acceptedTcpClient = tcpListener.AcceptTcpClient();
                Console.WriteLine("Accepted new client connection");
                new Thread(() => HandleClient(acceptedTcpClient)).Start();
            }
        }

        private void HandleClient(TcpClient acceptedTcpClient)
        {
            var isClientConnected = true;
            try
            {
                //Obtener el stream de la conexion
                var networkStream = acceptedTcpClient.GetStream();

                while (isClientConnected)
                {
                    var headerBytes = new byte[Protocol.HeaderLength];
                    var totalReceived = 0;
                    while (totalReceived < Protocol.HeaderLength)
                    {
                        var received = networkStream.Read(headerBytes, totalReceived, Protocol.WordLength - totalReceived);
                        if (received == 0) // if receive 0 bytes this means that connection was interrupted between the two points
                        {
                            throw new SocketException();
                        }
                        totalReceived += received;
                    }
                    var header = Encoding.UTF8.GetString(headerBytes);
                    Console.WriteLine("HEADER: " + header);
                    var dataLength = new byte[Protocol.WordLength];
                    totalReceived = 0;
                    while (totalReceived < Protocol.WordLength)
                    {
                        var received = networkStream.Read(dataLength, totalReceived, Protocol.WordLength - totalReceived);
                        if (received == 0) // if receive 0 bytes this means that connection was interrupted between the two points
                        {
                            throw new SocketException();
                        }
                        totalReceived += received;
                    }
                    var length = BitConverter.ToInt32(dataLength, 0); // Toma los 4 bytes a partir de la posicion indicada y los convierte
                    var data = new byte[length];
                    totalReceived = 0;
                    while (totalReceived < length)
                    {
                        var received = networkStream.Read(data, totalReceived, length - totalReceived);
                        if (received == 0)
                        {
                            throw new SocketException();
                        }
                        totalReceived += received;
                    }
                    var word = Encoding.UTF8.GetString(data);
                    if (word.Equals("exit"))
                    {
                        isClientConnected = false;
                        Console.WriteLine("Client is leaving");
                    }
                    else
                    {
                        Console.WriteLine("Client says: " + word);
                    }
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine($"The client connection was interrupted, message {se.Message}");
            }
        }
    }
}
