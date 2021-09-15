using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Utils.CustomExceptions
{
    public class InvalidGameException : Exception
    {
        public override string Message => "Game was not registered in the system";
    }
}
