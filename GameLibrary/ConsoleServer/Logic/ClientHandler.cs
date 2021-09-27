using Common.NetworkUtils;
using Common.Protocol;
using ConsoleServer.Logic.Commands.Factory;
using ConsoleServer.Logic.Commands.Strategies;
using System;
using System.Collections.Generic;


namespace ConsoleServer.Logic
{
    public class ClientHandler
    {
        public static bool stopHandling;

        private Dictionary<SocketHandler, string> _loggedClients;
        private static readonly object _padlock = new object();
        private static ClientHandler _instance;
        private int _clientClosedConnectionAbruptly = 0;

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
                    if (header.ICommand == _clientClosedConnectionAbruptly)
                    {
                        isSocketActive = false;
                        CloseConnection(clientSocketHandler);
                    }
                    else
                    {
                        CommandStrategy commandStrategy = CommandFactory.GetStrategy(header.ICommand);
                        commandStrategy.HandleRequest(header, clientSocketHandler);
                    }
                }
                catch (SocketClientException)
                {
                    isSocketActive = false;
                    CloseConnection(clientSocketHandler);
                    Console.WriteLine($"Se perdio la conexion con un socket");
                }
                catch (FormatException)
                {
                    isSocketActive = false;
                    CloseConnection(clientSocketHandler);
                    Console.WriteLine($"Error en formato de protocolo, cerrando conexion con el cliente");
                }
            }
        }

        private void CloseConnection(SocketHandler clientSocketHandler)
        {
            _loggedClients.Remove(clientSocketHandler);
            clientSocketHandler.ShutdownSocket();
        }
    }
}
