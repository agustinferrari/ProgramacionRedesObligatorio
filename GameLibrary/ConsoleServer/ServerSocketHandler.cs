using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using ConsoleServer.Logic;
using ConsoleServer.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

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
            _tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
        }

        public async Task CreateClientConectionTask()
        {
            Task.Run(async () => await ListenForConnections().ConfigureAwait(false));
        }

        public void CloseConections()
        {
            ClientHandler.stopHandling = true;
            Exit = true;
            foreach (SocketHandler client in ClientsConnectedSockets)
            {
                client.ShutdownSocket();
            }
            _tcpListener.Stop();
        }

        private async Task ListenForConnections()
        {
            ClientsConnectedSockets = new List<ISocketHandler>();
            while (!Exit)
            {
                try
                {
                    IClientHandler clientHandler = ClientHandler.Instance;
                    _tcpListener.Start(_supportedConnections);
                    TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync().ConfigureAwait(false);
                    _tcpListener.Stop();
                    ISocketHandler clientConnectedHandler = new SocketHandler(tcpClient.GetStream());
                    ClientsConnectedSockets.Add(clientConnectedHandler);
                    Console.WriteLine("Nueva conexion aceptada...");
                    //Ver si iba el await despues de => o no
                    var task = Task.Run(async () => clientHandler.HandleClient(clientConnectedHandler).ConfigureAwait(false));
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
