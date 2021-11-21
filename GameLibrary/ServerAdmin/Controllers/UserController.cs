using System.Threading.Tasks;
using CommonModels;
using Microsoft.AspNetCore.Mvc;
using ServerAdmin.ServicesGrpc;
using ServerAdmin.ServicesGrpcInterfaces;

namespace ServerAdmin.Controllers
{  
    [Route("api/users")]
    [ApiController]
    public class UserController
    {
        private readonly IUserGrpc _userServiceGrpc;
        
        public UserController(IUserGrpc users)
        {
            _userServiceGrpc = users;
        }
        
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
        
        [HttpPost ("{gameToBuy}")]
        public async Task<IActionResult> Post([FromHeader] string userAsking, string gameToBuy)
        {
            string response = await _userServiceGrpc.BuyGame(userAsking, gameToBuy);
            return new OkObjectResult(response);
        }
        
        [HttpDelete ("{gameToDeleteForUser}")]
        public async Task<IActionResult> DeleteGameForUser([FromHeader] string userAsking, string gameToDeleteForUser)
        {
            string response = await _userServiceGrpc.DeleteGameForUser(userAsking, gameToDeleteForUser);
            return new OkObjectResult(response);
        }
        
        [HttpPut]
        public async Task<IActionResult> Put([FromHeader] string userAsking, UserModel NewUserName)
        {
            string response = await _userServiceGrpc.ModifyUser(userAsking, NewUserName);
            return new OkObjectResult(response);
        }
    }
}