using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utils.CustomExceptions
{
    class UnableToReadFileException : Exception
    {
        public override string Message => "The file is not accesible";
    }
}
