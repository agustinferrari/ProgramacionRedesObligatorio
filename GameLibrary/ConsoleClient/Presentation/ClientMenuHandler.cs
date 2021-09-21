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
                case "5":
                    HandleGameReview(clientSocket);
                    break;
                case "7":
                    HandleGetGameDetails(clientSocket);
                    break;
                case "6":
                    HandleListOwnedGames(clientSocket);
                    break;
                default:
                    Console.WriteLine("La opcion seleccionada es invalida.");
                    LoadMainMenu(clientSocket);
                    break;
            }
        }

        private static void LoadLoggedUserMenu(SocketHandler clientSocket)
        {
            _fileHandler = new FileHandler();
            ClientMenuRenderer.RenderLoggedUserMenu();
            HandleLoggedUserMenuResponse(clientSocket);
        }

        private static string SendMessageAndRecieveResponse(SocketHandler clientSocket, int command, string messageToSend)
        {
            clientSocket.SendMessage(HeaderConstants.Request, command, messageToSend);
            return RecieveResponse(clientSocket);
        }

        private static string RecieveResponse(SocketHandler clientSocket)
        {
            Header header = clientSocket.ReceiveHeader();
            string response = clientSocket.ReceiveString(header.IDataLength);
            return response;
        }

        private static void HandleLogin(SocketHandler clientSocket)
        {
            Console.WriteLine("Por favor ingrese el nombre de usuario para logearse: ");
            string user = Console.ReadLine().ToLower();
            if (ValidateNotEmptyFields(user))
            {
                clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.Login, user);
                Header header = clientSocket.ReceiveHeader();
                string response = clientSocket.ReceiveString(header.IDataLength);
                Console.WriteLine(response);
                if (response == ResponseConstants.LoginSuccess)
                    LoadLoggedUserMenu(clientSocket);
                else
                    LoadMainMenu(clientSocket);
            }
            else
            {
                LoadMainMenu(clientSocket);
            }
        }

        private static void HandleLogout(SocketHandler clientSocket)
        {
            Header header = new Header(HeaderConstants.Request, CommandConstants.Logout, 0);
            clientSocket.SendHeader(header);
            string response = RecieveResponse(clientSocket);
            Console.WriteLine(response);
            if (response == ResponseConstants.LogoutSuccess)
                LoadMainMenu(clientSocket);
            else
                LoadLoggedUserMenu(clientSocket);
        }

        private static void HandleListGames(SocketHandler clientSocket)
        {
            Console.WriteLine("Desea filtrar la lista de juegos ? \n Y/N");
            string filterResponse = Console.ReadLine().ToLower();
            if (filterResponse == "y")
                HandleListGamesFiltered(clientSocket);
            else
            {
                Header header = new Header(HeaderConstants.Request, CommandConstants.ListGames, 0);
                clientSocket.SendHeader(header);
                Header recivedHeader = clientSocket.ReceiveHeader();
                string response = clientSocket.ReceiveString(recivedHeader.IDataLength);
                Console.WriteLine("Lista de juegos:");
                Console.WriteLine(response);
            }
        }

        private static void HandleListGamesFiltered(SocketHandler clientSocket)
        {
            Console.WriteLine("Por favor ingrese titulo a filtrar, si no desea esta opción, ingrese enter:");
            string filterTitle = Console.ReadLine().ToLower();
            Console.WriteLine("Por favor ingrese genero a filtrar, si no desea esta opción, ingrese enter:");
            string genreFIlter = Console.ReadLine().ToLower();
            Console.WriteLine("Por favor ingrese rating minimo a filtrar, si no desea esta opción, ingrese enter:");
            string ratingTitle = Console.ReadLine().ToLower();
            string totalFilter = filterTitle + "%" + genreFIlter + "%" + ratingTitle;
            string response = SendMessageAndRecieveResponse(clientSocket, CommandConstants.ListFilteredGames, totalFilter);
            Console.WriteLine("Lista de juegos:");
            Console.WriteLine(response);
            LoadLoggedUserMenu(clientSocket);
        }

        private static void HandleListOwnedGames(SocketHandler clientSocket)
        {
            Header header = new Header(HeaderConstants.Request, CommandConstants.ListOwnedGames, 0);
            clientSocket.SendHeader(header);
            string response = RecieveResponse(clientSocket);
            Console.WriteLine("Lista de juegos propios:");
            Console.WriteLine(response);
            LoadLoggedUserMenu(clientSocket);
        }

        private static void HandleBuyGame(SocketHandler clientSocket)
        {
            Console.WriteLine("Por favor ingrese el nombre del juego para comprar:");
            string gameName = Console.ReadLine().ToLower();
            if (ValidateNotEmptyFields(gameName))
            {
                string response = SendMessageAndRecieveResponse(clientSocket, CommandConstants.BuyGame, gameName);
                Console.WriteLine(response);
                if (response == ResponseConstants.BuyGameSuccess || response == ResponseConstants.InvalidGameError || response == ResponseConstants.GameAlreadyBought)
                    LoadLoggedUserMenu(clientSocket);
                else
                    LoadMainMenu(clientSocket);
            }
            else
            {
                LoadLoggedUserMenu(clientSocket);
            }
        }

        private static void HandleAddGame(SocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese el nombre del juego:");
            string name = Console.ReadLine();
            Console.WriteLine("Ingrese el genero del juego:");
            string genre = Console.ReadLine();
            Console.WriteLine("Ingrese un resumen del juego:");
            string synopsis = Console.ReadLine();
            Console.WriteLine("Ingrese el path de la caratula del juego que desea subir:");
            string path = Console.ReadLine();

            string gameData = name + "%" + genre + "%" + synopsis;
            if (ValidateNotEmptyFields(gameData))
            {
                clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.AddGame, gameData);
                clientSocket.SendImage(path);

                Header recivedHeader = clientSocket.ReceiveHeader();
                string response = clientSocket.ReceiveString(recivedHeader.IDataLength);
                Console.WriteLine(response);
                if (response == ResponseConstants.AddGameSuccess)
                    LoadLoggedUserMenu(clientSocket);
                else
                    LoadMainMenu(clientSocket);
            }
            else
            {
                LoadLoggedUserMenu(clientSocket);
            }
        }

        private static void HandleGameReview(SocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese el nombre del juego:");
            string gameName = Console.ReadLine();
            Console.WriteLine("Ingrese el rating del juego (1-10):");
            string rating = Console.ReadLine();
            Console.WriteLine("Ingrese un comentario acerca del juego:");
            string comment = Console.ReadLine();
            string review = gameName + "%" + rating + "%" + comment;
            if (ValidateNotEmptyFields(gameName))
            {
                string response = SendMessageAndRecieveResponse(clientSocket, CommandConstants.ReviewGame, review);
                Console.WriteLine(response);
                if (response == ResponseConstants.ReviewGameSuccess || response == ResponseConstants.InvalidGameError
                        || response == ResponseConstants.InvalidRatingException)
                    LoadLoggedUserMenu(clientSocket);
                else
                    LoadMainMenu(clientSocket);
            }
            else
            {
                LoadLoggedUserMenu(clientSocket);
            }
        }

        private static void HandleGetGameDetails(SocketHandler clientSocket)
        {
            Console.WriteLine("Ingrese el nombre del juego para ver sus detalles:");
            string gameName = Console.ReadLine();
            if (ValidateNotEmptyFields(gameName))
            {
                clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.GetGameDetails, gameName);
                Header header = clientSocket.ReceiveHeader();
                string response = clientSocket.ReceiveString(header.IDataLength);
                Console.WriteLine(response);
                if (response != ResponseConstants.InvalidGameError)
                {
                    Console.WriteLine("Para descargar la caratula ingrese 1:");
                    string option = Console.ReadLine();
                    if (option == "1")
                    {
                        clientSocket.SendMessage(HeaderConstants.Request, CommandConstants.GetGameImage, gameName);
                        Header recivedHeader = clientSocket.ReceiveHeader();//Capaz que sacarlo
                        string rawImageData = clientSocket.ReceiveString(SpecificationHelper.GetImageDataLength());
                        string pathToImageGame = clientSocket.ReceiveImage(rawImageData);
                        Console.WriteLine("La foto fue guardada en: " + pathToImageGame);
                    }
                }
            }
            LoadLoggedUserMenu(clientSocket);
        }

        private static bool ValidateNotEmptyFields(string data)
        {
            string[] separatedData = data.Split("%");
            foreach (string field in separatedData)
                if (field == "")
                {
                    Console.WriteLine("Por favor rellene todos los campos");
                    return false;
                }
            return true;
        }
    }
}
