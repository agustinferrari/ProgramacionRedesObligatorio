using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public static class CommandConstants
    {
        public const int Login = 1;
        public const int ListUsers = 2;
        public const int Message = 3;
        public const int Logout = 4;
        public const int LoginError = 5;
        public const int LoginSuccess = 6;
    }
}
