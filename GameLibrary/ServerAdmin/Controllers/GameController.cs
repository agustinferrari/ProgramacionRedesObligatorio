
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAdmin.GrpcControllers;

namespace ServerAdmin.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController
    {
        private GameGrpc gamesController = new GameGrpc();
        public GameController()
        {
            //gamesController = gameController;
        }

        [HttpGet]
        //hay que hacerlos await? entonces el GetGames de controller tambien?
        public async Task<IActionResult> Get()
        {
            string games = await gamesController.GetGames();
            return new OkObjectResult(games);
        }
    }
}