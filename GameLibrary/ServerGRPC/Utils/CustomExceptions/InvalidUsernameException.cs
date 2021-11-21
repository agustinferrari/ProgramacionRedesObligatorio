using System;

namespace ServerGRPC.Utils.CustomExceptions
{
    public class InvalidUsernameException : Exception
    {
        public override string Message => "Usuario no registrado en sistema";
    }
}
