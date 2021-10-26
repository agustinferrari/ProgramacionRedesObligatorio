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
using System.IO;
using System.Threading.Tasks;

namespace ConsoleClient.Menu.Logic
{
    public class ClientMenuHandler : IClientMenuHandler
    {
        public async Task LoadMainMenu(INetworkStreamHandler clientNetworkStream)
        {
            ClientMenuRenderer.RenderMainMenu();
            await HandleMainMenuResponse(clientNetworkStream);
        }
        public async Task LoadLoggedUserMenu(INetworkStreamHandler clientNetworkStream)
        {
            ClientMenuRenderer.RenderLoggedUserMenu();
            await HandleLoggedUserMenuResponse(clientNetworkStream);
        }

        private async Task HandleMainMenuResponse(INetworkStreamHandler clientNetworkStream)
        {
            string selectedOption = Console.ReadLine();
            Console.Clear();
            try
            {
                int parsedOption = ParseMainMenuOption(selectedOption);
                if (parsedOption == CommandConstants.Login || parsedOption == CommandConstants.ListGames)
                {
                    MenuStrategy menuStrategy = MenuFactory.GetStrategy(parsedOption);
                    string response = await menuStrategy.HandleSelectedOption(clientNetworkStream);
                    Console.WriteLine(response);
                    if (response == ResponseConstants.LoginSuccess)
                        await LoadLoggedUserMenu(clientNetworkStream);
                    else
                        await LoadMainMenu(clientNetworkStream);
                }
                else
                {
                    Console.WriteLine("La opcion seleccionada es invalida.");
                    await LoadMainMenu(clientNetworkStream);
                }
            }
            catch (Exception e) when (e is IOException || e is AggregateException)
            {
                Console.WriteLine("Se perdio la conexion con el server, intente mas tarde");
            }
        }

        private async Task HandleLoggedUserMenuResponse(INetworkStreamHandler clientNetworkStream)
        {
            string selectedOption = Console.ReadLine();
            Console.Clear();
            try
            {
                int parsedOption = ParseLoggedUserMenuOption(selectedOption);
                if (parsedOption >= CommandConstants.ListGames && parsedOption <= CommandConstants.DeletePublishedGame)
                {
                    MenuStrategy menuStrategy = MenuFactory.GetStrategy(parsedOption);
                    string response = await menuStrategy.HandleSelectedOption(clientNetworkStream);
                    Console.WriteLine(response);
                    if (response == ResponseConstants.LogoutSuccess || response == ResponseConstants.InvalidUsernameError
                        || response == ResponseConstants.AuthenticationError)
                        await LoadMainMenu(clientNetworkStream);
                    else
                        await LoadLoggedUserMenu(clientNetworkStream);
                }
                else
                {
                    Console.WriteLine("La opcion seleccionada es invalida.");
                    await LoadLoggedUserMenu(clientNetworkStream);
                }
            }
            catch (Exception e) when (e is IOException || e is AggregateException)
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
