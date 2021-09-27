using Common.FileUtils;
using Common.FileUtils.Interfaces;
using Common.NetworkUtils;
using Common.Protocol;
using ConsoleClient.Menu.Logic.Factory;
using ConsoleClient.Menu.Logic.Interfaces;
using ConsoleClient.Menu.Logic.Strategies;
using ConsoleClient.Menu.Presentation;
using System;
using System.Net.Sockets;

namespace ConsoleClient.Menu.MenuHandler
{
    public class ClientMenuHandler : IClientMenuHandler
    {
        private IFileHandler _fileHandler;
        private static readonly object _padlock = new object();
        private static ClientMenuHandler _instance;

        private ClientMenuHandler()
        {
            _fileHandler = new FileHandler();
        }

        public static ClientMenuHandler Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ClientMenuHandler();
                    }
                    return _instance;
                }
            }
        }
        public void LoadMainMenu(SocketHandler clientSocket)
        {
            ClientMenuRenderer.RenderMainMenu();
            HandleMainMenuResponse(clientSocket);
        }

        private void HandleMainMenuResponse(SocketHandler clientSocket)
        {

            string selectedOption = Console.ReadLine();
            Console.Clear();
            try
            {
                int parsedOption = ParseMainMenuOption(selectedOption);
                if (parsedOption >= CommandConstants.Login && parsedOption <= CommandConstants.Logout)
                {
                    MenuStrategy menuStrategy = MenuFactory.GetStrategy(parsedOption);
                    menuStrategy.HandleSelectedOption(clientSocket);

                }
                else
                {
                    Console.WriteLine("La opcion seleccionada es invalida.");
                    LoadMainMenu(clientSocket);
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Se perdio la conexion con el server, intente mas tarde");
            }
        }

        private int ParseMainMenuOption(string selectedOption)
        {
            int result;
            try
            {
                result = Int32.Parse(selectedOption);
            }
            catch (FormatException)
            {
                result = CommandConstants.InvalidOption;
            }
            return result;
        }

        private void HandleLoggedUserMenuResponse(SocketHandler clientSocket)
        {
            string selectedOption = Console.ReadLine();
            Console.Clear();
            try
            {
                int parsedOption = ParseLoggedUserMenuOption(selectedOption);
                if (parsedOption >= CommandConstants.ListGames && parsedOption <= CommandConstants.ModifyPublishedGame)
                {
                    MenuStrategy menuStrategy = MenuFactory.GetStrategy(parsedOption);
                    menuStrategy.HandleSelectedOption(clientSocket);
                }
                else
                {
                    Console.WriteLine("La opcion seleccionada es invalida.");
                    LoadLoggedUserMenu(clientSocket);
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Se perdio la conexion con el server, intente mas tarde");
            }
        }

        private int ParseLoggedUserMenuOption(string selectedOption)
        {
            int result;
            int mainMenuOptions = 2;
            try
            {
                result = Int32.Parse(selectedOption) + mainMenuOptions;
            }
            catch (FormatException)
            {
                result = CommandConstants.InvalidOption;
            }
            return result;
        }

        public void LoadLoggedUserMenu(SocketHandler clientSocket)
        {
            ClientMenuRenderer.RenderLoggedUserMenu();
            HandleLoggedUserMenuResponse(clientSocket);
        }

        public string SendMessageAndRecieveResponse(SocketHandler clientSocket, int command, string messageToSend)
        {
            clientSocket.SendMessage(HeaderConstants.Request, command, messageToSend);
            return RecieveResponse(clientSocket);
        }

        public string RecieveResponse(SocketHandler clientSocket)
        {
            string response;
            try
            {
                Header header = clientSocket.ReceiveHeader();
                response = clientSocket.ReceiveString(header.IDataLength);
            }
            catch (FormatException)
            {
                response = "No se pudo decodificar correctamente";
            }
            return response;
        }

        public void HandleListGamesFiltered(SocketHandler clientSocket)
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
        }

        public bool ValidateNotEmptyFields(string data)
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
