using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Presentation
{
    public static class ClientMenuRenderer
    {
        public static void RenderMainMenu()
        {
            RenderTopSeparator();
            Console.WriteLine("Bienvenido a Game Library!");
            Console.WriteLine("Por favor seleccione una opción:");
            Console.WriteLine("1. Login -> iniciar sesión en el servidor");
            Console.WriteLine("2. Ver catalogo de juegos -> muestra el catalogo de todos los juegos registrados");
            Console.WriteLine("Ingrese su opcion: ");
            RenderBottomSeparator();
        }

        public static void RenderLoggedUserMenu()
        {
            RenderTopSeparator();
            Console.WriteLine("Gracias por ingresar a Game Library!");
            Console.WriteLine("Por favor seleccione una opción:");
            Console.WriteLine("1. Logout -> cerrar sesion en el servidor");
            Console.WriteLine("2. Ver catalogo de juegos -> muestra el catalogo de todos los juegos registrados");
            Console.WriteLine("3. Comprar juego -> permite comprar un juego");
            Console.WriteLine("4. Publicar juego -> permite publicar un juego");
            Console.WriteLine("5. Calificar juego -> permite calificar un juego adquirido por el usuario");
            Console.WriteLine("6. Ver biblioteca -> permite ver la biblioteca de juegos adquiridos por el usuario");
            Console.WriteLine("7. Ver detalle de juego -> permite ver el detalle de un juego en especifico");
            Console.WriteLine("Ingrese su opcion: ");
            RenderBottomSeparator();
        }

        private static void RenderTopSeparator()
        {
            Console.WriteLine("-----------------------------------------------------------------------------------");
            Console.WriteLine("");
        }

        private static void RenderBottomSeparator()
        {
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------------------------------------");
        }

    }
}
