using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Utils.CustomExceptions
{
    public class GameDoesNotExistOnLibraryExcpetion : Exception
    {
        public override string Message => "Game was not bought by this user";
    }
}
