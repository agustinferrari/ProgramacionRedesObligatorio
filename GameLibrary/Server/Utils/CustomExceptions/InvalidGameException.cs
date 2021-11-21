using System;


namespace Server.Utils.CustomExceptions
{
    public class InvalidGameException : Exception
    {
        public override string Message => "Juego no registrado en el sistema";
    }
}
