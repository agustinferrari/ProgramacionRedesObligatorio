using System;

namespace ConsoleServer.Presentation
{
    public static class ServerMenuHandler
    {
        public static void HandleMainMenuResponse(ServerSocketHandler serverSocket)
        {
            bool exit = false;

            while (!exit)
            {
                string selectedOption = Console.ReadLine();
                switch (selectedOption)
                {
                    case "1":
                        serverSocket.CloseConections();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("La opcion seleccionada es invalida.");
                        break;
                }
            }

            Console.WriteLine("Exiting server...");
        }
    }
}
