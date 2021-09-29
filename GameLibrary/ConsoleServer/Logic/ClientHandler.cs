using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using Common.Utils.CustomExceptions;
using ConsoleServer.Logic.Commands.Factory;
using ConsoleServer.Logic.Commands.Strategies;
using ConsoleServer.Logic.Interfaces;
using System;
using System.Collections.Generic;


namespace ConsoleServer.Logic
{
    public class ClientHandler : IClientHandler
    {
        public static bool stopHandling;

        private Dictionary<ISocketHandler, string> _loggedClients;
        private static readonly object _padlock = new object();
        private static ClientHandler _instance;
        private int _clientClosedConnectionAbruptly = 0;

        private ClientHandler()
        {
            _loggedClients = new Dictionary<ISocketHandler, string>();
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

        public bool IsSocketInUse(ISocketHandler socketHandler)
        {
            lock (_padlock)
                return _loggedClients.ContainsKey(socketHandler);
        }

        public string GetUsername(ISocketHandler socketHandler)
        {
            lock (_padlock)
                return _loggedClients[socketHandler];
        }

        public void AddClient(ISocketHandler socketHandler, string userName)
        {
            lock (_padlock)
                _loggedClients.Add(socketHandler, userName);
        }

        public void RemoveClient(ISocketHandler socketHandler)
        {
            lock (_padlock)
                _loggedClients.Remove(socketHandler);
        }

        public void HandleClient(ISocketHandler clientSocketHandler)
        {
            bool isSocketActive = true;
            while (!stopHandling && isSocketActive)
            {
                isSocketActive = clientSocketHandler.IsSocketClosed();
                try
                {
                    Header header = clientSocketHandler.ReceiveHeader();
                    if (header.ICommand == _clientClosedConnectionAbruptly)
                    {
                        if (isSocketActive)
                            CloseConnection(clientSocketHandler);
                        isSocketActive = false;
                    }
                    else
                    {
                        CommandStrategy commandStrategy = CommandFactory.GetStrategy(header.ICommand);
                        commandStrategy.HandleRequest(header, clientSocketHandler);
                    }
                }
                catch (SocketClientException)
                {
                    if (isSocketActive)
                        CloseConnection(clientSocketHandler);
                    isSocketActive = false;
                    Console.WriteLine($"Se perdio la conexion con un socket");
                }
                catch (FormatException)
                {
                    if (isSocketActive)
                        CloseConnection(clientSocketHandler);
                    isSocketActive = false;
                    Console.WriteLine($"Error en formato de protocolo, cerrando conexion con el cliente");
                }
            }
        }

        private void CloseConnection(ISocketHandler clientSocketHandler)
        {
            _loggedClients.Remove(clientSocketHandler);
            clientSocketHandler.ShutdownSocket();
        }
    }
}
