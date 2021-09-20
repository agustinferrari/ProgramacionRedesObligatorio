using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Utils.CustomExceptions
{
    public class GameAlreadyBoughtException : Exception
    {
        public override string Message => "Game was already bought by this user";
    }
}
