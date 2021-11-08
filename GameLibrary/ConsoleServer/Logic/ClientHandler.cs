using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using Common.Utils.CustomExceptions;
using ConsoleServer.Logic.Commands.Factory;
using ConsoleServer.Logic.Commands.Strategies;
using ConsoleServer.Logic.Interfaces;
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using CommonLog;

namespace ConsoleServer.Logic
{
    public class ClientHandler : IClientHandler
    {
        public static bool stopHandling;

        private Dictionary<INetworkStreamHandler, string> _loggedClients;
        private static readonly object _padlock = new object();
        private static ClientHandler _instance;
        private int _clientClosedConnectionAbruptly = 0;

        private ClientHandler()
        {
            _loggedClients = new Dictionary<INetworkStreamHandler, string>();
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

        public bool IsSocketInUse(INetworkStreamHandler networkStreamHandler)
        {
            lock (_padlock)
                return _loggedClients.ContainsKey(networkStreamHandler);
        }

        public string GetUsername(INetworkStreamHandler networkStreamHandler)
        {
            lock (_padlock)
                return _loggedClients[networkStreamHandler];
        }

        public void AddClient(INetworkStreamHandler networkStreamHandler, string userName)
        {
            lock (_padlock)
                _loggedClients.Add(networkStreamHandler, userName);
        }

        public void RemoveClient(INetworkStreamHandler networkStreamHandler)
        {
            lock (_padlock)
                _loggedClients.Remove(networkStreamHandler);
        }

        public async Task HandleClient(INetworkStreamHandler clientNetworkStreamHandler)
        {
            bool isSocketActive = true;
            while (!stopHandling && isSocketActive)
            {
                try
                {
                    Header header = await clientNetworkStreamHandler.ReceiveHeader();
                    if (header.ICommand == _clientClosedConnectionAbruptly)
                    {
                        CloseConnection(clientNetworkStreamHandler);
                        isSocketActive = false;
                    }
                    else
                    {
                        CommandStrategy commandStrategy = CommandFactory.GetStrategy(header.ICommand);
                        await commandStrategy.HandleRequest(header, clientNetworkStreamHandler);
                    }
                }
                catch (Exception e) when (e is IOException || e is AggregateException)
                {
                    CloseConnection(clientNetworkStreamHandler);
                    isSocketActive = false;
                    Console.WriteLine($"Se perdio la conexion con un socket");
                }
                catch (Exception e) when (e is FormatException || e is KeyNotFoundException)
                {
                    CloseConnection(clientNetworkStreamHandler);
                    isSocketActive = false;
                    Console.WriteLine($"Error en formato de protocolo, cerrando conexion con el cliente");
                }
            }
        }

        private void CloseConnection(INetworkStreamHandler clientNetworkStreamHandler)
        {
            _loggedClients.Remove(clientNetworkStreamHandler);
            clientNetworkStreamHandler.ShutdownSocket();
        }
    }
}