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


    }
}
