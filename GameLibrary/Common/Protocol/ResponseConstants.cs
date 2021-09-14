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
    }
}
