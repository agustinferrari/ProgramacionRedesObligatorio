
using System.Threading.Tasks;
using CommonModels;
using Microsoft.AspNetCore.Mvc;
using ServerAdmin.ServicesGrpc;

namespace ServerAdmin.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController
    {
        private readonly GameGrpc _gameServiceGrpc = new GameGrpc();
        public GameController()
        {
            //GameServiceGrpc = gameController;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader] string user)
        {
            string games = await _gameServiceGrpc.GetGames(user);
            return new OkObjectResult(games);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GameModel Game)
        {
            string response = await _gameServiceGrpc.AddGame(Game);
            return new OkObjectResult(response);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete([FromHeader] string user, [FromQuery] string game)
        {
            string gameDeleted = await _gameServiceGrpc.DeleteGame(user, game);
            return new OkObjectResult(gameDeleted);
        }
        
        // [HttpPut]
        // public async Task<IActionResult> Put()
        // {
        //     // string games = await GameServiceGrpc.GetGames();
        //     // return new OkObjectResult(games);
        // }
    }
}