using System;


namespace ServerGRPC.Utils.CustomExceptions
{
    public class GameAlreadyBoughtException : Exception
    {
        public override string Message => "Game was already bought by this user";
    }
}
