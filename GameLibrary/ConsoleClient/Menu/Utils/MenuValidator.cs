using ConsoleClient.Menu.Utils.Interfaces;
using System;

namespace ConsoleClient.Menu.Utils
{
    public class MenuValidator : IMenuValidator
    {
        public bool ValidateNotEmptyFields(string data)
        {
            string[] separatedData = data.Split("%");
            foreach (string field in separatedData)
                if (field == "")
                {
                    Console.WriteLine("Por favor rellene todos los campos");
                    return false;
                }
            return true;
        }
    }
}
