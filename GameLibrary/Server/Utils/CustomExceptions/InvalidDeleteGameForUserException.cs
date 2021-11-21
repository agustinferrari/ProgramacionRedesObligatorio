using System;


namespace Server.Utils.CustomExceptions
{
    public class InvalidDeleteGameForUserException : Exception
    {
        public override string Message => "Juego no adquirido por este usuario";
    }
}
