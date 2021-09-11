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
        public static List<Socket> _clientsConnectedSockets { get; set; }

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
            foreach (Socket client in _clientsConnectedSockets)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            var fakeSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            fakeSocket.Connect("127.0.0.1", 20000);
        }

        private void ListenForConnections(Socket socketServer)
        {
            _clientsConnectedSockets = new List<Socket>();
            while (!exit)
            {
                try
                {
                    Socket clientConnected = socketServer.Accept();
                    _clientsConnectedSockets.Add(clientConnected);
                    Console.WriteLine("Accepted new connection...");
                    Thread threadcClient = new Thread(() => ClientHandler.HandleClient(clientConnected));
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
