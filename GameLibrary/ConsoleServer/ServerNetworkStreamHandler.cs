﻿using Common.NetworkUtils;
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
    public class ServerNetworkStreamHandler : NetworkStreamHandler
    {
        public bool Exit { get; set; }
        private List<INetworkStreamHandler> ClientsConnectedSockets { get; set; }
        private int _supportedConnections = 100;
        private TcpListener _tcpListener;

        public ServerNetworkStreamHandler(string ipAddress, int port) :
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
            foreach (NetworkStreamHandler client in ClientsConnectedSockets)
            {
                client.ShutdownSocket();
            }
            _tcpListener.Stop();
        }

        private async Task ListenForConnections()
        {
            ClientsConnectedSockets = new List<INetworkStreamHandler>();
            while (!Exit)
            {
                try
                {
                    IClientHandler clientHandler = ClientHandler.Instance;
                    _tcpListener.Start(_supportedConnections);
                    TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync().ConfigureAwait(false);
                    _tcpListener.Stop();
                    INetworkStreamHandler clientConnectedHandler = new NetworkStreamHandler(tcpClient.GetStream());
                    ClientsConnectedSockets.Add(clientConnectedHandler);
                    Console.WriteLine("Nueva conexion aceptada...");
                    Task.Run(async () => await clientHandler.HandleClient(clientConnectedHandler).ConfigureAwait(false));
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