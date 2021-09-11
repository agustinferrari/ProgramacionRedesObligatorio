using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Presentation
{
    public static class ServerMenuRenderer
    {
        public static void LoadMainMenu()
        {
            Console.WriteLine("Bienvenido a GameLibrary Server");
            Console.WriteLine("Opciones validas: ");
            Console.WriteLine("1. exit -> abandonar el programa");
            Console.WriteLine("Ingrese su opcion: ");
        }
    }
}
