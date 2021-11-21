
using System.Threading.Tasks;
using CommonModels;
using Microsoft.AspNetCore.Mvc;
using ServerAdmin.ServicesGrpc;
using ServerAdmin.ServicesGrpcInterfaces;

namespace ServerAdmin.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController
    {
        private readonly IGameGrpc _gameServiceGrpc;
        public GameController(IGameGrpc service)
        {
            _gameServiceGrpc = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader] string userAsking)
        {
            string games = await _gameServiceGrpc.GetGames(userAsking);
            return new OkObjectResult(games);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GameModel Game)
        {
            string response = await _gameServiceGrpc.AddGame(Game);
            return new OkObjectResult(response);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete([FromHeader] string userAsking, [FromQuery] string game)
        {
            string gameDeleted = await _gameServiceGrpc.DeleteGame(userAsking, game);
            return new OkObjectResult(gameDeleted);
        }
        
        [HttpPut ("{gameToModify}")]
        public async Task<IActionResult> Put(string gameToModify ,[FromBody] GameModel Game)
        {
            string response = await _gameServiceGrpc.ModifyGame(gameToModify,Game);
            return new OkObjectResult(response);
        }
    }
}