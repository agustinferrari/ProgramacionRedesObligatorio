using Common.NetworkUtils;
using Common.Protocol;
using ConsoleServer.BussinessLogic;
using ConsoleServer.Domain;
using ConsoleServer.Logic.Commands.Factory;
using ConsoleServer.Logic.Commands.Strategies;
using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConsoleServer.Logic
{
    public class ClientHandler
    {
        public static bool stopHandling;

        private Dictionary<SocketHandler, string> _loggedClients;
        private static readonly object _padlock = new object();
        private static ClientHandler _instance;

        public ClientHandler()
        {
            _loggedClients = new Dictionary<SocketHandler, string>();
            stopHandling = false;
        }

        public static ClientHandler Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ClientHandler();
                    }
                    return _instance;
                }
            }
        }

        public bool IsClientLogged(string userName)
        {
            lock (_padlock)
                return _loggedClients.ContainsValue(userName);
        }

        public bool IsSocketInUse(SocketHandler socketHandler)
        {
            lock (_padlock)
                return _loggedClients.ContainsKey(socketHandler);
        }
       
        public string GetUsername(SocketHandler socketHandler)
        {
            lock (_padlock)
                return _loggedClients[socketHandler];
        }

        public void AddClient(SocketHandler socketHandler, string userName)
        {
            lock (_padlock)
                _loggedClients.Add(socketHandler, userName);
        }

        public void RemoveClient(SocketHandler socketHandler)
        {
            lock (_padlock)
                _loggedClients.Remove(socketHandler);
        }

        public void HandleClient(SocketHandler clientSocketHandler)
        {
            bool isSocketActive = true;
            while (!stopHandling && isSocketActive)
            {
                try
                {
                    Header header = clientSocketHandler.ReceiveHeader();
                    if (header.ICommand == 0) // Cuando el cliente cierra la consola, se envia en header con command en 0
                    {
                        isSocketActive = false;
                        _loggedClients.Remove(clientSocketHandler);
                        clientSocketHandler.ShutdownSocket();
                    }
                    else
                    {
                        CommandStrategy commandStrategy = CommandFactory.GetStrategy(header.ICommand);
                        commandStrategy.HandleRequest(header, clientSocketHandler);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Server is closing, will not process more data -> Message {e.Message}..");
                }
            }
        }
    }
}
