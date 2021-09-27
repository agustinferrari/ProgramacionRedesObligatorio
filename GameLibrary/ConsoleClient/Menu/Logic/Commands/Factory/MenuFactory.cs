using Common.Protocol;
using ConsoleClient.Menu.Logic.Commands.Strategies;
using System.Collections.Generic;

namespace ConsoleClient.Menu.Logic.Commands.Factory
{
    public static class MenuFactory
    {
        private static Dictionary<int, MenuStrategy> _commandMap = new Dictionary<int, MenuStrategy>()
        {
            { CommandConstants.Login, new Login() },
            { CommandConstants.ListGames, new ListGamesMainMenu()},
            { CommandConstants.Logout, new Logout()},
            { CommandConstants.ListGamesLoggedUser, new ListGamesLoggedUser()},
            { CommandConstants.BuyGame, new BuyGame()},
            { CommandConstants.AddGame, new AddGame()},
            { CommandConstants.ReviewGame, new GameReview()},
            { CommandConstants.ListOwnedGames, new ListOwnedGames()},
            { CommandConstants.GetGameDetails, new GetGameDetails()},
            { CommandConstants.ModifyPublishedGame, new ModifyPublishedGame()},
            { CommandConstants.DeletePublishedGame, new DeletePublishedGame()},
        };

        public static MenuStrategy GetStrategy(int commandConstant)
        {
            return _commandMap[commandConstant];
        }
    }
}
