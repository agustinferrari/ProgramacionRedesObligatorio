using System;

namespace CommonProtocol.Utils.CustomExceptions
{
    public class SocketClientException : Exception
    {
        public override string Message => "Lost connection with client socket";
    }
}
