using ConsoleClient.Presentation;
using System;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientMenuRenderer.loadMainMenu();
            ClientMenuHandler.handleMainMenuResponse();
            Console.WriteLine("Hello World!");
        }
    }
}
