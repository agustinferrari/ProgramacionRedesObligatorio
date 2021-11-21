using System;

namespace Server.Presentation
{
    public static class ServerMenuRenderer
    {
        public static void LoadMainMenu()
        {
            Console.WriteLine("Bienvenido a GameLibrary Server");
            Console.WriteLine("Opciones validas: ");
            Console.WriteLine("1. Salir del sistema -> abandonar el programa");
            Console.WriteLine("Ingrese su opcion: ");
        }
    }
}
