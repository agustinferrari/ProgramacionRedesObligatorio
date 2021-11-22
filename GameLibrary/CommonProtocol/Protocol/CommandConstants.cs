using System;
using System.Collections.Generic;
using System.Text;

namespace CommonProtocol.Protocol
{
    public static class CommandConstants
    {
        public const int Login = 1;
        public const int ListGames = 2;
        public const int Logout = 3;
        public const int BuyGame = 4;
        public const int AddGame = 5;
        public const int ReviewGame = 6;
        public const int ListOwnedGames = 7;
        public const int GetGameDetails = 8;
        public const int ModifyPublishedGame = 9;
        public const int DeletePublishedGame = 10;
        public const int GetGameImage = 11;
        public const int ListFilteredGames = 12;
        public const int AddUser = 13;
        public const int ModifyUser = 14;
        public const int DeleteUser = 15;
        public const int DeleteOwnedGame = 16;
        public const int InvalidOption = -1;
    }
}
