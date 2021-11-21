using System;

namespace ServerGRPC.Utils.CustomExceptions
{
    public class GameAlreadyAddedException : Exception
    {
        public override string Message => "Este juego ya se encuentre en el sistema";
    }
}
