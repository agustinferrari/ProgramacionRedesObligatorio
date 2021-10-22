using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using ConsoleServer.Logic;
using ConsoleServer.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleServer
{
    public class ServerSocketHandler : SocketHandler
    {
        public bool Exit { get; set; }
        private List<ISocketHandler> ClientsConnectedSockets { get; set; }
        private int _supportedConnections = 100;
        private TcpListener _tcpListener;

        public ServerSocketHandler(string ipAddress, int port) :
            base()
        {
            //_networkStream.Listen(_supportedConnections);
            _tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
        }

        public void CreateClientConectionThread()
        {
            Thread threadServer = new Thread(() => ListenForConnections());
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
            _networkStream.Close(0);
        }

        private void ListenForConnections()
        {
            ClientsConnectedSockets = new List<ISocketHandler>();
            while (!Exit)
            {
                try
                {
                    IClientHandler clientHandler = ClientHandler.Instance;
                    _tcpListener.Start(1);
                    TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                    _tcpListener.Stop();
                    //Socket clientConnected = socketServer.Accept();
                    ISocketHandler clientConnectedHandler = new SocketHandler(tcpClient.GetStream());
                    ClientsConnectedSockets.Add(clientConnectedHandler);
                    Console.WriteLine("Nueva conexion aceptada...");
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
