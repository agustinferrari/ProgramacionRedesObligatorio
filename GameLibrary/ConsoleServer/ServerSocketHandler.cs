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
        public bool Exit { get; set; }
        private List<SocketHandler> ClientsConnectedSockets { get; set; }
        private int _supportedConnections = 100;

        public ServerSocketHandler(string ipAddress, int port) :
            base(ipAddress, port)
        {
            _socket.Listen(_supportedConnections);
        }

        public void CreateClientConectionThread()
        {
            Thread threadServer = new Thread(() => ListenForConnections(_socket));
            threadServer.Start();
        }

        public void CloseConections()
        {
            ClientHandler.stopHandling = true;
            Exit = true;
            foreach (SocketHandler client in ClientsConnectedSockets)
            {
                client.ShutdownSocket();
            }
            var fakeSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            fakeSocket.Connect(_ipAddress, _port);
            _socket.Close(0);
        }

        private void ListenForConnections(Socket socketServer)
        {
            ClientsConnectedSockets = new List<SocketHandler>();
            while (!Exit)
            {
                try
                {
                    ClientHandler clientHandler = new ClientHandler();
                    Socket clientConnected = socketServer.Accept();
                    SocketHandler clientConnectedHandler = new SocketHandler(clientConnected);
                    ClientsConnectedSockets.Add(clientConnectedHandler);
                    Console.WriteLine("Accepted new connection...");
                    Thread threadcClient = new Thread(() => clientHandler.HandleClient(clientConnectedHandler));
                    threadcClient.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Exit = true;
                }
            }
            Console.WriteLine("Exiting....");
        }
    }
}
