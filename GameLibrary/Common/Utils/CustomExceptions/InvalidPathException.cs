using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utils.CustomExceptions
{
    class InvalidPathException : Exception
    {
        public override string Message => "The entered path is not accesible";
    }
}
