using System;
using System.Collections.Generic;
using System.Text;

namespace LogsModels
{
    public class CommandDictionary
    {
        private static Dictionary<int, string> _commandMap = new Dictionary<int, string>()
        {
            { 1, "Login" },
            { 2, "ListGames"},
            { 3, "Logout" },
            { 4, "BuyGame"},
            { 5, "AddGame" },
            { 6, "ReviewGame"},
            { 7, "ListOwnedGames" },
            { 8, "GetGameDetails"},
            { 9, "ModifyPublishedGame" },
            { 10, "DeletePublishedGame"},
            { 11, "GetGameImage" },
            { 12, "ListFilteredGames"},
        };

        public static string ParseCommand(int command)
        {
            if (_commandMap.ContainsKey(command))
            {
                return _commandMap[command];
            }
            return null;
        }
    }
}
