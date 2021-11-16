
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLogs.LogsStorage.GameLogs;

namespace ServerLogs.Controllers
{
    [Route("api/gamelogs")]
    [ApiController]
    public class GameLogsController : ControllerBase
    {
       private readonly Games _context;

        public GameLogsController()
        {
            _context = Games.Instance;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameModel>>> GetGameLogs()
        {
            return _context.GetLogs();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<GameModel>> GetGameLogs(int id)
        {
            var gameLog = _context.GetLog(id);
            if (gameLog == null)
            {
                return NotFound();
            }
            
            return gameLog;
        }
        
        [HttpPost]
        public async Task<ActionResult<GameModel>> PostGameLogs(GameModel game)
        {
            _context.AddGameLog(game);

            return CreatedAtAction(nameof(GetGameLogs), new { id = game.Id }, game);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameModel>> DeleteGameLog(int id)
        {
            var gameLog = _context.DeleteLog(id);
            if (gameLog == null)
            {
                return NotFound();
            }
            return gameLog;
        }
    }
}
