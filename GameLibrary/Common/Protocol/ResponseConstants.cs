using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public class ResponseConstants
    {
        public static string LoginSuccess = "Logeado correctamente";
        public static string LoginErrorAlreadyLogged = "El usuario ya esta logeado";
        public static string LoginErrorSocketAlreadyInUse = "El socket ya esta en uso";
        public static string LogoutSuccess = "Se ha cerrado sesion correctamente";
        public static string BuyGameSuccess = "Compra realizada exitosamente";
        public static string InvalidGameError = "El juego ingresado no se encuentra en el sistema, intente de nuevo";
        public static string InvalidUsernameError = "El usuario no se encuentra en el sistema, intente de nuevo";
        public static string AuthenticationError = "Debe logearse para comprar un juego";
        public static string ReviewGameSuccess = "Calificacion realizada exitosamente";
        public static string InvalidRatingException = "El rating debe ser un entero entre 1 y 10, intente de nuevo";
        public static string LibraryError = "El usuario no posee juegos propios";
        public static string GameAlreadyBought = "El usuario ya compro este juego";
        public static string AddGameSuccess = "El libro se publico correctamente";
    }
}
