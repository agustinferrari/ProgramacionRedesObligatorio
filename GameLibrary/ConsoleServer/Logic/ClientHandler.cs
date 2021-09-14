using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ConsoleServer.Logic
{
    public static class ClientHandler
    {
        private static Dictionary<SocketHandler, string> loggedClients;
        public static void HandleClient(SocketHandler clientSocketHandler)
        {
            loggedClients = new Dictionary<SocketHandler, string>();
            bool isSocketActive = true;
            while (!ServerSocketHandler.exit && isSocketActive)
            {
                try
                {
                    Header header = clientSocketHandler.ReceiveHeader();
                    switch (header.ICommand)
                    {
                        case CommandConstants.Login:
                            HandleLogin(header, clientSocketHandler);
                            break;
                        case CommandConstants.Logout:
                            HandleLogout(header, clientSocketHandler);
                            break;
                        case 0:
                            isSocketActive = false;
                            loggedClients.Remove(clientSocketHandler);
                            clientSocketHandler.ShutdownSocket();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Server is closing, will not process more data -> Message {e.Message}..");
                }
            }
        }

        private static void HandleLogin(Header header, SocketHandler clientSocketHandler)
        {
            string userName = clientSocketHandler.ReceiveString(header.IDataLength); //Podriamos hacer un metodo que haga todo esto de una
            string responseMessageResult;
            if (loggedClients.ContainsValue(userName))
            {
                responseMessageResult = ResponseConstants.LoginErrorAlreadyLogged;
            }
            else
            {
                if (!loggedClients.ContainsKey(clientSocketHandler))
                {
                    loggedClients.Add(clientSocketHandler, userName);
                    responseMessageResult = ResponseConstants.LoginSuccess;
                }
                else
                {
                    responseMessageResult = ResponseConstants.LoginErrorSocketAlreadyInUse;
                }
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.Login, responseMessageResult);
        }

        private static void HandleLogout(Header header, SocketHandler clientSocketHandler)
        {
            if (loggedClients.ContainsKey(clientSocketHandler))
                loggedClients.Remove(clientSocketHandler);
            string responseMessageResult = ResponseConstants.LogoutSuccess;
            clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.Logout, responseMessageResult);
        }
    }
}
