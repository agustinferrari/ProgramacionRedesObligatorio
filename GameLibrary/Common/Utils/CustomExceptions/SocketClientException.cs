using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utils.CustomExceptions
{
    class SocketClientException : Exception
    {
        public override string Message => "Lost connection with client socket";
    }
}
