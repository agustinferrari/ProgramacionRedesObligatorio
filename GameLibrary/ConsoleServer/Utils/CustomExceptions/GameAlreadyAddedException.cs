using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Utils.CustomExceptions
{
    public class GameAlreadyAddedException : Exception
    {
        public override string Message => "Game was already added";
    }
}
