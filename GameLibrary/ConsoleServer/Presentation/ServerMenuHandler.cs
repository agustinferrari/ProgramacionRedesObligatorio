using System;

namespace ConsoleServer.Presentation
{
    public static class ServerMenuHandler
    {
        public static void HandleMainMenuResponse(ServerNetworkStreamHandler serverNetworkStreamHandler)
        {
            bool exit = false;

            while (!exit)
            {
                string selectedOption = Console.ReadLine();
                switch (selectedOption)
                {
                    case "1":
                        serverNetworkStreamHandler.CloseConections();
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
