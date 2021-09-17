﻿using Common.NetworkUtils;
using ConsoleServer.Logic;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleServer
{
    public class ServerSocketHandler : SocketHandler
    {
        public bool exit { get; set; }
        private List<SocketHandler> _clientsConnectedSockets { get; set; }
        private ManualResetEvent _manualResetEvent;
        private string _ipAddress;
        private int _port;

        public ServerSocketHandler(string ipAddress, int port) :
            base(ipAddress, port)
        {
            _ipAddress = ipAddress;
            _port = port;
            _socket.Listen(100);
        }

        public void CreateClientConectionThread()
        {
            Thread threadServer = new Thread(() => ListenForConnections(_socket));
            threadServer.Start();
        }

        public void CloseConections()
        {
            _manualResetEvent.Set();
            exit = true;
            foreach (SocketHandler client in _clientsConnectedSockets)
            {
                client.ShutdownSocket();
            }
            var fakeSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            fakeSocket.Connect(_ipAddress, _port);
            _socket.Close(0);
        }

        private void ListenForConnections(Socket socketServer)
        {
            _manualResetEvent = new ManualResetEvent(false);
            _clientsConnectedSockets = new List<SocketHandler>();
            while (!exit)
            {
                try
                {
                    Socket clientConnected = socketServer.Accept();
                    SocketHandler clientConnectedHandler = new SocketHandler(clientConnected);
                    _clientsConnectedSockets.Add(clientConnectedHandler);
                    Console.WriteLine("Accepted new connection...");
                    Thread threadcClient = new Thread(() => ClientHandler.HandleClient(clientConnectedHandler, _manualResetEvent));
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
