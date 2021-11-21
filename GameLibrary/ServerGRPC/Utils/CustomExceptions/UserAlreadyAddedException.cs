using System;


namespace ServerGRPC.Utils.CustomExceptions
{
    public class UserAlreadyAddedException : Exception
    {
        public override string Message => "Este usuario ya se encuentre en el sistema";
    }
}
