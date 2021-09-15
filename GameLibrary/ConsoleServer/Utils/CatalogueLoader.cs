﻿using ConsoleServer.BussinessLogic;
using ConsoleServer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Utils
{
    public class CatalogueLoader
    {
        public static void AddGames(GameController gameController)
        {
            Game newGame = new Game
            {
                Name = "Minecraft",
                Genre = "Building",
                PathToPhoto = "ahtddstd",
                Synopsis = "A building world made of blocks",
                Rating = 10
            };
            Game newGame1 = new Game
            {
                Name = "Call of Duty",
                Genre = "Shooter",
                PathToPhoto = "ahtddstd",
                Synopsis = "First person online shooter",
                Rating = 5
            };
            Game newGame2 = new Game
            {
                Name = "Fifa",
                Genre = "Football",
                PathToPhoto = "ahtddstd",
                Synopsis = "Footbal game",
                Rating = 3
            };
            Game newGame3 = new Game
            {
                Name = "Hades",
                Genre = "Roguelike",
                PathToPhoto = "ahtddstd",
                Synopsis = "Son of Hades escaped underworld",
                Rating = 7
            };
            gameController.AddGame(newGame);
            gameController.AddGame(newGame1);
            gameController.AddGame(newGame2);
            gameController.AddGame(newGame3);
        }
    }
}
