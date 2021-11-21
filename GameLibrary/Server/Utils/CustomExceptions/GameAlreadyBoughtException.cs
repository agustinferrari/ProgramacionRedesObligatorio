using System;


namespace Server.Utils.CustomExceptions
{
    public class GameAlreadyBoughtException : Exception
    {
        public override string Message => "Este usuario ya posee este juego";
    }
}
