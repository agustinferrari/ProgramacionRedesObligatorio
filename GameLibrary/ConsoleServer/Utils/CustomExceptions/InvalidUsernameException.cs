﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Utils.CustomExceptions
{
    public class InvalidUsernameException : Exception
    {
        public override string Message => "User was not registered in the system";
    }
}
