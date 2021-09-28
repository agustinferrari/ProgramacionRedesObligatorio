using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Menu.Utils.Interfaces
{
    public interface IMenuValidator
    {
        public bool ValidateNotEmptyFields(string data);
    }
}
