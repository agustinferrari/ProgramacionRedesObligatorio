using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public static class CommandConstants
    {
        public const int Login = 1;
        public const int Logout = 2;
        public const int ListGames = 3;
        public const int BuyGame = 4;
        public const int AddGame = 5;
        public const int ReviewGame = 6;
        public const int GetGameDetails = 7;
        public const int GetGameImage = 8;
        public const int ListOwnedGames = 9;
        public const int ListFilteredGames = 10;
        public const int DeleteOwnedGame = 11;
        public const int ModifyOwnedGame = 12;
    }
}
