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
        public async Task<IActionResult> GetAll([FromHeader] string userAsking)
        {
            string games = await _userServiceGrpc.GetUsers(userAsking);
            return new OkObjectResult(games);
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Post([FromHeader] string userAsking,[FromBody] UserModel UserToAdd)
        {
            string response = await _userServiceGrpc.AddModifyUser(userAsking, UserToAdd);
            return new OkObjectResult(response);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete([FromHeader] string userAsking, [FromQuery] string userToDelete)
        {
            string userDeleted = await _userServiceGrpc.DeleteUser(userAsking, userToDelete);
            return new OkObjectResult(userDeleted);
        }
        
        // [HttpPut]
        // public async Task<IActionResult> Put()
        // {
        //     // string games = await gamesController.GetGames();
        //     // return new OkObjectResult(games);
        // }
    }
}