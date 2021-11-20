using System.Threading.Tasks;
using CommonModels;
using Microsoft.AspNetCore.Mvc;
using ServerAdmin.ServicesGrpc;

namespace ServerAdmin.Controllers
{  
    [Route("api/users")]
    [ApiController]
    public class UserController
    {
        private readonly UserGrpc _userServiceGrpc = new UserGrpc();
        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader] string user)
        {
            string games = await _userServiceGrpc.GetUsers(user);
            return new OkObjectResult(games);
        }
        
        //
        // [HttpPost]
        // public async Task<IActionResult> Post([FromBody] GameModel Game)
        // {
        //     string response = await gamesController.AddGame(Game);
        //     return new OkObjectResult(response);
        // }
        //
        // [HttpDelete]
        // public async Task<IActionResult> Delete([FromHeader] string user, [FromQuery] string game)
        // {
        //     string gameDeleted = await gamesController.DeleteGame(user, game);
        //     return new OkObjectResult(gameDeleted);
        // }
        
        // [HttpPut]
        // public async Task<IActionResult> Put()
        // {
        //     // string games = await gamesController.GetGames();
        //     // return new OkObjectResult(games);
        // }
    }
}