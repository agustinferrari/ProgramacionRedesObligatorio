using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ConsoleServer.Logic
{
    public static class  ClientHandler
    {
        private static Dictionary<Socket, string> clientsLogged;
        public static void HandleClient(Socket clientConnected)
        {
            clientsLogged = new Dictionary<Socket, string>();
            while (!ServerSocketHandler.exit)
            {
                int headerLength = HeaderConstants.Request.Length + HeaderConstants.CommandLength +
                                   HeaderConstants.DataLength;
                byte[] buffer = new byte[headerLength];
                try
                {
                    ReceiveData(clientConnected, headerLength, buffer);
                    Header header = new Header();
                    header.DecodeData(buffer);
                    switch (header.ICommand)
                    {
                        case CommandConstants.Login:
                            HandleLogin(header, clientConnected);
                            break;
                        case CommandConstants.ListUsers:
                            Console.WriteLine("Not Implemented yet...");
                            break;
                        case CommandConstants.Message:
                            Console.WriteLine("Will receive message to display...");
                            var bufferData = new byte[header.IDataLength];
                            ReceiveData(clientConnected, header.IDataLength, bufferData);
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

        private static void HandleLogin(Header header, Socket clientConnected)
        {
            byte[] bufferData = new byte[header.IDataLength];
            ReceiveData(clientConnected, header.IDataLength, bufferData);
            string userName = Encoding.UTF8.GetString(bufferData);
            if (clientsLogged.ContainsValue(userName))
            {
                Console.WriteLine("User already logged in! " );
            }
            else
            {
                if (!clientsLogged.ContainsKey(clientConnected))
                {
                    clientsLogged.Add(clientConnected, userName);
                    Console.WriteLine("New user logged in! " + userName);
                }
                else
                    Console.WriteLine("Socket already in use!");
            }
        }

        private static void ReceiveData(Socket clientSocket, int Length, byte[] buffer)
        {
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
        }
    }

}
