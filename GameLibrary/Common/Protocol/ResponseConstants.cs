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
        public static string AuthenticationError = "Debe logearse para acceder a esta opcion";
        public static string ReviewGameSuccess = "Calificacion realizada exitosamente";
        public static string InvalidRatingException = "El rating debe ser un entero entre 1 y 10, intente de nuevo";
        public static string LibraryError = "El usuario no posee juegos propios";
        public static string GameAlreadyBought = "El usuario ya compro este juego";
        public static string AddGameSuccess = "El juego se publico correctamente";
        public static string AddGameError = "El juego ya se encuentra en el sistema";
        public static string DeleteGameSuccess = "El juego ha sido borrado correctamente";
        public static string DeleteGameError= "El juego  no ha sido borrado correctamente";
        public static string UnauthorizedGame = "El juego no pertence a este usuario";
        public static string ModifyPublishedGameSuccess = "El juego ha sido editado correctamente";
        public static string NoAvailableGames = "No hay juegos cargados en el sistema";
    }
}
