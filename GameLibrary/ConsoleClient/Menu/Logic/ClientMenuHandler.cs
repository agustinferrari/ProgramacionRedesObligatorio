using Common.FileUtils;
using Common.FileUtils.Interfaces;
using Common.NetworkUtils.Interfaces;
using Common.Protocol;
using ConsoleClient.Menu.Logic.Commands.Factory;
using ConsoleClient.Menu.Logic.Interfaces;
using ConsoleClient.Menu.Logic.Commands.Strategies;
using ConsoleClient.Menu.Presentation;
using System;
using System.Net.Sockets;

namespace ConsoleClient.Menu.Logic
{
    public class ClientMenuHandler : IClientMenuHandler
    {
        public void LoadMainMenu(ISocketHandler clientSocket)
        {
            ClientMenuRenderer.RenderMainMenu();
            HandleMainMenuResponse(clientSocket);
        }
        public void LoadLoggedUserMenu(ISocketHandler clientSocket)
        {
            ClientMenuRenderer.RenderLoggedUserMenu();
            HandleLoggedUserMenuResponse(clientSocket);
        }

        private void HandleMainMenuResponse(ISocketHandler clientSocket)
        {
            string selectedOption = Console.ReadLine();
            Console.Clear();
            try
            {
                int parsedOption = ParseMainMenuOption(selectedOption);
                if (parsedOption == CommandConstants.Login || parsedOption == CommandConstants.ListGames)
                {
                    MenuStrategy menuStrategy = MenuFactory.GetStrategy(parsedOption);
                    string response = menuStrategy.HandleSelectedOption(clientSocket);
                    Console.WriteLine(response);
                    if (response == ResponseConstants.LoginSuccess)
                        LoadLoggedUserMenu(clientSocket);
                    else
                        LoadMainMenu(clientSocket);

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

        private void HandleLoggedUserMenuResponse(ISocketHandler clientSocket)
        {
            string selectedOption = Console.ReadLine();
            Console.Clear();
            try
            {
                int parsedOption = ParseLoggedUserMenuOption(selectedOption);
                if (parsedOption >= CommandConstants.ListGames && parsedOption <= CommandConstants.DeletePublishedGame)
                {
                    MenuStrategy menuStrategy = MenuFactory.GetStrategy(parsedOption);
                    string response = menuStrategy.HandleSelectedOption(clientSocket);
                    Console.WriteLine(response);
                    if (response == ResponseConstants.LogoutSuccess || response == ResponseConstants.LogoutSuccess
                        || response == ResponseConstants.AuthenticationError)
                        LoadMainMenu(clientSocket);
                    else
                        LoadLoggedUserMenu(clientSocket);
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
            int mainMenuOptions = 1;
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
