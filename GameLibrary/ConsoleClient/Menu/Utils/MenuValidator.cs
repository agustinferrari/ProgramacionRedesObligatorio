using ConsoleClient.Menu.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Utils
{
    public class MenuValidator : IMenuValidator
    {
        public bool ValidateNotEmptyFields(string data)
        {
            return true;
        }
    }
}
