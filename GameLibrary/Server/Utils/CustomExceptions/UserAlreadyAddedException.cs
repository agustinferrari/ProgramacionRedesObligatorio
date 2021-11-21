using System;


namespace Server.Utils.CustomExceptions
{
    public class UserAlreadyAddedException : Exception
    {
        public override string Message => "Este usuario ya se encuentre en el sistema";
    }
}
