using Common.FileUtils;
using Common.FileUtils.Interfaces;
using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ConsoleClient.Presentation
{
    public static class ClientMenuHandler
    {
        private static IFileHandler _fileHandler;
        public static void LoadMainMenu(SocketHandler clientSocket)
        {
            ClientMenuRenderer.RenderMainMenu();
            HandleMainMenuResponse(clientSocket);

        }

        private static void HandleMainMenuResponse(SocketHandler clientSocket)
        {

            string selectedOption = Console.ReadLine();
            try
            {
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
                        LoadMainMenu(clientSocket);
                        break;
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine("Se perdio la conexion con el server, intente mas tarde");
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
            Header header = new Header(HeaderConstants.Request, CommandConstants.ListGames, 0);
            clientSocket.SendHeader(header);
            Header recivedHeader = clientSocket.ReceiveHeader();
            string response = clientSocket.ReceiveString(recivedHeader.IDataLength);
            Console.WriteLine("Lista de juegos:");
            Console.WriteLine(response);
        }

        private static void LoadLoggedUserMenu(SocketHandler clientSocket)
        {
            _fileHandler = new FileHandler();
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
                case "4":
                    HandleAddGame(clientSocket);
                    break;
                default:
                    Console.WriteLine("La opcion seleccionada es invalida.");
                    LoadMainMenu(clientSocket);
                    break;
            }
        }

        private static void HandleLogout(SocketHandler clientSocket)
        {
            Header header = new Header(HeaderConstants.Request, CommandConstants.Logout, 0);
            clientSocket.SendHeader(header);
            Header recivedHEader = clientSocket.ReceiveHeader();
            string response = clientSocket.ReceiveString(recivedHEader.IDataLength);
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

        private static void HandleAddGame(SocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese el nombre del juego");
            string name = Console.ReadLine();
            Console.WriteLine("Ingrese el genero del juego");
            string genre = Console.ReadLine();
            Console.WriteLine("Ingrese un resumen del juego");
            string synopsis = Console.ReadLine();
            Console.WriteLine("Ingrese el path de la caratula del juego que desea subir");
            string path = Console.ReadLine();

            string gameData = name + "%" + genre + "%" + synopsis;
            string fileName = _fileHandler.GetFileName(path);
            long fileSize = _fileHandler.GetFileSize(path);
            clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.AddGame, gameData);

            clientSocket.SendImage(path);

           
            LoadMainMenu(clientSocket);
        }
    }
}
