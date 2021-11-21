using System;


namespace ServerGRPC.Utils.CustomExceptions
{
    public class InvalidDeleteGameForUserException : Exception
    {
        public override string Message => "Juego no adquirido por este usuario";
    }
}
