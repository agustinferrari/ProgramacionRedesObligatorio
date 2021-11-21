using Common.NetworkUtils;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LogsModels;
using Server.Logic.Commands.Factory;
using Server.Logic.Commands.Strategies;
using Server.Logic.Interfaces;
using Server.Logic.LogManager;

namespace Server.Logic
{
    public class ClientHandler : IClientHandler
    {
        public static bool stopHandling;

        private Dictionary<INetworkStreamHandler, string> _loggedClients;
        private static readonly object _padlock = new object();
        private static ClientHandler _instance;
        private static LogLogic _logLogic;
        private int _clientClosedConnectionAbruptly = 0;

        private ClientHandler()
        {
            _loggedClients = new Dictionary<INetworkStreamHandler, string>();
            stopHandling = false;
            _logLogic = LogLogic.Instance;
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
                        LogGameModel log = await commandStrategy.HandleRequest(header, clientNetworkStreamHandler);
                        _logLogic.SendLog(log);
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