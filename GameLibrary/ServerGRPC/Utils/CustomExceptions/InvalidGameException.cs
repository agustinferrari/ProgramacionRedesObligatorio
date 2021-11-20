using System;


namespace ServerGRPC.Utils.CustomExceptions
{
    public class InvalidGameException : Exception
    {
        public override string Message => "Juego no registrado en el sistema";
    }
}
