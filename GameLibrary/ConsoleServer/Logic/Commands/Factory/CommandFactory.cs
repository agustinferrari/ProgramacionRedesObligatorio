using ConsoleServer.Logic.Commands.Strategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Logic.Commands.Factory
{
    public static class CommandFactory
    {
        private static Dictionary<int, CommandStrategy> _commandMap = new Dictionary<int, CommandStrategy>()
        {
            { 1, new Login() },
            { 2, new Logout() },
            { 3, new ListGames() },
            { 4, new BuyGame() },
            { 5, new AddGame() },
            { 6, new ReviewGame() },
            { 7, new GetGameDetails() },
            { 8, new GetGameImage() },
            { 9, new ListOwnedGames() },
            { 10, new ListFilteredGames() },
            { 11, new DeleteGamePublished() },
            { 12, new ModifyGamePublished() },
        };

        public static CommandStrategy GetStrategy(int commandConstant)
        {
            return _commandMap[commandConstant];
        }
    }
}
