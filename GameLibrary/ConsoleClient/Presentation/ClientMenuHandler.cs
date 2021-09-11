using Common.NetworkUtils;
using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Presentation
{
    public static class ClientMenuHandler
    {
        public static void HandleMainMenuResponse(SocketHandler clientSocket)
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
            ClientMenuRenderer.LoadLoggedUserMenu();
            HandleSecondaryMenu();
        }

        private static void HandleSecondaryMenu()
        {
            while (true)
            {

            }
        }
    }
}
