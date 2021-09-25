using System;
using System.Collections.Generic;
using System.Text;

namespace Common.NetworkUtils
{
    public class PortValidator
    {
        public static bool Validate(string port)
        {
            bool result;
            try
            {
                int parsedPort = Int32.Parse(port);
                if (parsedPort > 0 && parsedPort < 65536)
                    result = true;
                else
                    result = false;
            }
            catch (FormatException)
            {
                result = false;
            }
            return result;
        }
    }
}
