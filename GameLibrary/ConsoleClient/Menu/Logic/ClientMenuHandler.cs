using Common.FileUtils;
using Common.FileUtils.Interfaces;
using Common.NetworkUtils;
using Common.Protocol;
using ConsoleClient.Menu.Presentation;
using System;
using System.Net.Sockets;

namespace ConsoleClient.Menu.MenuHandler
{
    public class ClientMenuHandler
    {
        private IFileHandler _fileHandler;

        public ClientMenuHandler()
        {
            _fileHandler = new FileHandler();
        }

        private static ClientMenuHandler _instance;

        public static ClientMenuHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClientMenuHandler();
                return _instance;
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
                if (parsedOption > 0 && parsedOption < 3)
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
            catch (SocketException se)
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
            catch (FormatException e)
            {
                result = -1;
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
                if (parsedOption > 2 && parsedOption < 12)
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
            catch (SocketException se)
            {
                Console.WriteLine("Se perdio la conexion con el server, intente mas tarde");
            }
        }

        private int ParseLoggedUserMenuOption(string selectedOption)
        {
            int result;
            try
            {
                result = Int32.Parse(selectedOption) + 2; //Sacar magic number, es la cantidad de opciones del main menu
            }
            catch (FormatException e)
            {
                result = -1;
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
            Header header = clientSocket.ReceiveHeader();
            string response = clientSocket.ReceiveString(header.IDataLength);
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
