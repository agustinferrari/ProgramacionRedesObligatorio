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
                /*int headerLength = HeaderConstants.Request.Length + HeaderConstants.CommandLength +
                                   HeaderConstants.DataLength;
                byte[] buffer = new byte[headerLength];*/
                try
                {
                    /*clientSocketHandler.ReceiveData(headerLength, buffer);
                    Header header = new Header();
                    header.DecodeData(buffer);*/
                    Header header = clientSocketHandler.ReceiveHeader();
                    switch (header.ICommand)
                    {
                        case CommandConstants.Login:
                            HandleLogin(header, clientSocketHandler);
                            break;
                        case CommandConstants.ListUsers:
                            Console.WriteLine("Not Implemented yet...");
                            break;
                        case CommandConstants.Message:
                            Console.WriteLine("Will receive message to display...");
                            var bufferData = new byte[header.IDataLength];
                            clientSocketHandler.ReceiveData(header.IDataLength, bufferData);
                            Console.WriteLine("Message received: " + Encoding.UTF8.GetString(bufferData));
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
            if (loggedClients.ContainsValue(userName))
            {
                Console.WriteLine("El usuario ya esta logeado!");
                clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.Login, "El usuario ya esta logeado!");
            }
            else
            {
                if (!loggedClients.ContainsKey(clientSocketHandler))
                {
                    loggedClients.Add(clientSocketHandler, userName);
                    Console.WriteLine("Nuevo usuario logeado " + userName);
                    clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.Login, "Logeado correctamente");
                }
                else
                {
                    Console.WriteLine("El socket ya esta en uso");
                    clientSocketHandler.SendMessage(HeaderConstants.Response, CommandConstants.Login, "El socket ya esta en uso");
                }
            }
        }

        /*private static void ReceiveData(SocketHandler clientSocketHandler, int Length, byte[] buffer)
        {
            //Ver si meter esto en socket asi _socket queda protected
            Socket clientSocket = clientSocketHandler._socket;
            var iRecv = 0;
            while (iRecv < Length)
            {
                try
                {
                    var localRecv = clientSocket.Receive(buffer, iRecv, Length - iRecv, SocketFlags.None);
                    if (localRecv == 0) // Si recieve retorna 0 -> la conexion se cerro desde el endpoint remoto
                    {
                        if (!ServerSocketHandler.exit)
                        {
                            clientSocket.Shutdown(SocketShutdown.Both);
                            clientSocket.Close();
                        }
                        else
                        {
                            throw new Exception("Server is closing");
                        }
                    }

                    iRecv += localRecv;
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.Message);
                    return;
                }
            }
        }*/
    }

}
