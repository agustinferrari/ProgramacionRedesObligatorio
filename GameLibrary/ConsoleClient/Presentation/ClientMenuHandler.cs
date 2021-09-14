using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Presentation
{
    public static class ClientMenuHandler
    {
        public static void LoadMainMenu(SocketHandler clientSocket)
        {
            ClientMenuRenderer.RenderMainMenu();
            HandleMainMenuResponse(clientSocket);
        }

        private static void HandleMainMenuResponse(SocketHandler clientSocket)
        {

            string selectedOption = Console.ReadLine();
            switch (selectedOption)
            {
                case "1":
                    HandleLogin(clientSocket);
                    break;
                case "2":
                    break;
                default:
                    Console.WriteLine("La opcion seleccionada es invalida.");
                    break;
            }
        }

        private static void HandleLogin(SocketHandler clientSocket)
        {
            Console.WriteLine("Por favor ingrese el nombre de usuario para logearse: ");
            string user = Console.ReadLine();
            clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.Login, user);
            Header header = clientSocket.ReceiveHeader();
            string response = clientSocket.ReceiveString(header.IDataLength);
            Console.WriteLine(response);
            if (header.ICommand == CommandConstants.LoginSuccess)
            {
                LoadLoggedUserMenu(clientSocket);
            }
            else
            {
                LoadMainMenu(clientSocket);
            }
        }

        private static void LoadLoggedUserMenu(SocketHandler clientSocket)
        {
            ClientMenuRenderer.RenderLoggedUserMenu();
            HandleLoggedUserMenuResponse(clientSocket);
        }

        private static void HandleLoggedUserMenuResponse(SocketHandler clientSocket)
        {
            string selectedOption = Console.ReadLine();
            switch (selectedOption)
            {
                case "1":
                    HandleLogin(clientSocket);
                    break;
                case "2":
                    break;
                default:
                    Console.WriteLine("La opcion seleccionada es invalida.");
                    break;
            }
        }
    }
}
