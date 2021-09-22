using ConsoleClient.Presentation.MenuHandler.Strategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleClient.Presentation.MenuHandler.Factory
{
    public static class MenuFactory
    {
        private static Dictionary<int, MenuStrategy> _commandMap = new Dictionary<int, MenuStrategy>()
        {
            { 1, new Login() },
            { 2, new ListGamesMainMenu()},
            { 3, new Logout()},
            { 4, new ListGamesLoggedUser()},
            { 5, new BuyGame()},
            { 6, new AddGame()},
            { 7, new GameReview()},
            { 8, new ListOwnedGames()},
            { 9, new GetGameDetails()},
            { 10, new ModifyOwnedGame()},
            { 11, new DeleteOwnedGame()},
        };

        public static MenuStrategy GetStrategy(int commandConstant)
        {
            return _commandMap[commandConstant];
        }
    }
}
