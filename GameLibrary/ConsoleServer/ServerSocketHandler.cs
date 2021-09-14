using Common.NetworkUtils;
using ConsoleServer.Logic;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleServer
{
    public class ServerSocketHandler : SocketHandler
    {
        public static bool exit { get; set; }
        private List<SocketHandler> _clientsConnectedSockets { get; set; }

        public ServerSocketHandler(string ipAddress, int port) :
            base(ipAddress, port)
        {
            _socket.Listen(100);
        }

        public void CreateClientConectionThread()
        {
            Thread threadServer = new Thread(() => ListenForConnections(_socket));
            threadServer.Start();
        }

        public void CloseConections()
        {
            exit = true;
            _socket.Close(0);
            foreach (SocketHandler client in _clientsConnectedSockets)
            {
                client.ShutdownSocket();
            }
            var fakeSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            fakeSocket.Connect("127.0.0.1", 20000);
        }

        private void ListenForConnections(Socket socketServer)
        {
            _clientsConnectedSockets = new List<SocketHandler>();
            while (!exit)
            {
                try
                {
                    Socket clientConnected = socketServer.Accept();
                    SocketHandler clientConnectedHandler = new SocketHandler(clientConnected);
                    _clientsConnectedSockets.Add(clientConnectedHandler);
                    Console.WriteLine("Accepted new connection...");
                    Thread threadcClient = new Thread(() => ClientHandler.HandleClient(clientConnectedHandler));
                    threadcClient.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    exit = true;
                }
            }
            Console.WriteLine("Exiting....");
        }
    }
}
