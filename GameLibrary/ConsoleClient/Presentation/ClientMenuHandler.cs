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
                    HandleListGames(clientSocket);
                    LoadMainMenu(clientSocket);
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
            if (response == ResponseConstants.LoginSuccess)
                LoadLoggedUserMenu(clientSocket);
            else
                LoadMainMenu(clientSocket);
        }

        private static void HandleListGames(SocketHandler clientSocket)
        {
            clientSocket.SendHeader(HeaderConstants.Request, CommandConstants.ListGames, 0);
            Header header = clientSocket.ReceiveHeader();
            string response = clientSocket.ReceiveString(header.IDataLength);
            Console.WriteLine("Lista de juegos:");
            Console.WriteLine(response);
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
                    HandleLogout(clientSocket);
                    break;
                case "2":
                    HandleListGames(clientSocket);
                    LoadLoggedUserMenu(clientSocket);
                    break;
                case "3":
                    HandleBuyGame(clientSocket);
                    break;
                default:
                    Console.WriteLine("La opcion seleccionada es invalida.");
                    break;
            }
        }

        private static void HandleLogout(SocketHandler clientSocket)
        {
            clientSocket.SendHeader(HeaderConstants.Request, CommandConstants.Logout, 0);
            Header header = clientSocket.ReceiveHeader();
            string response = clientSocket.ReceiveString(header.IDataLength);
            Console.WriteLine(response);
            if (response == ResponseConstants.LogoutSuccess)
                LoadMainMenu(clientSocket);
            else
                LoadLoggedUserMenu(clientSocket);
        }

        private static void HandleBuyGame(SocketHandler clientSocket)
        {
            Console.WriteLine("Por favor ingrese el nombre del juego para comprar: ");
            string gameName = Console.ReadLine();
            clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.BuyGame, gameName);
            Header header = clientSocket.ReceiveHeader();
            string response = clientSocket.ReceiveString(header.IDataLength);
            Console.WriteLine(response);
            if (response == ResponseConstants.BuyGameSuccess || response == ResponseConstants.InvalidGameError)
                LoadLoggedUserMenu(clientSocket);
            else
                LoadMainMenu(clientSocket);
        }
    }
}
