using Common.Protocol;
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
            { CommandConstants.Login, new Login() },
            { CommandConstants.Logout, new Logout() },
            { CommandConstants.ListGames, new ListGames() },
            { CommandConstants.ListGamesLoggedUser, new ListGames() },
            { CommandConstants.BuyGame, new BuyGame() },
            { CommandConstants.AddGame, new AddGame() },
            { CommandConstants.ReviewGame, new ReviewGame() },
            { CommandConstants.GetGameDetails, new GetGameDetails() },
            { CommandConstants.GetGameImage, new GetGameImage() },
            { CommandConstants.ListOwnedGames, new ListOwnedGames() },
            { CommandConstants.ListFilteredGames, new ListFilteredGames() },
            { CommandConstants.DeletePublishedGame, new DeleteGamePublished() },
            { CommandConstants.ModifyPublishedGame, new ModifyGamePublished() },
        };

        public static CommandStrategy GetStrategy(int commandConstant)
        {
            return _commandMap[commandConstant];
        }
    }
}
