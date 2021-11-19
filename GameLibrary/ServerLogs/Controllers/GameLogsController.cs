
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogsModels;
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
        public async Task<ActionResult<IEnumerable<LogGameModel>>> GetGameLogs()
        {
            return _context.GetLogs();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<LogGameModel>> GetGameLogs(int id)
        {
            var gameLog = _context.GetLog(id);
            if (gameLog == null)
            {
                return NotFound();
            }
            
            return gameLog;
        }
        
        [HttpPost]
        public async Task<ActionResult<LogGameModel>> PostGameLogs(LogGameModel logGame)
        {
            _context.AddGameLog(logGame);

            return CreatedAtAction(nameof(GetGameLogs), new { id = logGame.Id }, logGame);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<LogGameModel>> DeleteGameLog(int id)
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
