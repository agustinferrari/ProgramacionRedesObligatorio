using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Presentation
{
    public static class ClientMenuHandler
    {
        public static void handleMainMenuResponse()
        {
            string selectedOption = Console.ReadLine();
            switch (selectedOption)
            {
                case "1":
                    handleLogin();
                    break;
                case "2":
                    break;
                default:
                    Console.WriteLine("La opcion seleccionada es invalida.");
                    break;
            }
        }

        private static void handleLogin()
        {
            Console.WriteLine("Por favor ingrese el nombre de usuario para logearse: ");
        }
    }
}
