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
            while (!ServerSocketHandler.exit)
            {
                try
                {
                    Header header = clientSocketHandler.ReceiveHeader();
                    switch (header.ICommand)
                    {
                        case CommandConstants.Login:
                            HandleLogin(header, clientSocketHandler);
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
            int responseResult;
            if (loggedClients.ContainsValue(userName))
            {
                Console.WriteLine("El usuario ya esta logeado!");
                responseMessageResult = "El usuario ya esta logeado!";
                responseResult = CommandConstants.LoginError;
            }
            else
            {
                if (!loggedClients.ContainsKey(clientSocketHandler))
                {
                    loggedClients.Add(clientSocketHandler, userName);
                    Console.WriteLine("Nuevo usuario logeado " + userName);
                    responseMessageResult = "Logeado correctamente";
                    responseResult = CommandConstants.LoginSuccess;
                }
                else
                {
                    Console.WriteLine("El socket ya esta en uso");
                    responseMessageResult = "El socket ya esta en uso";
                    responseResult = CommandConstants.LoginError;
                }
            }
            clientSocketHandler.SendMessage(HeaderConstants.Response, responseResult, responseMessageResult);
        }
    }
}
