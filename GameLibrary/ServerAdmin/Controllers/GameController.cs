using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAdmin.GrpcControllers;

namespace ServerAdmin.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController
    {
        private GameGrpc gamesController;
        public GameController(GameGrpc gameController)
        {
            gamesController = gameController;
        }

        // [HttpGet]
        // public IEnumerable<GameModel> Get()
        // {
        //     //var games = gamesController.GetGames();
        //     //return games;
        // }
    }
}