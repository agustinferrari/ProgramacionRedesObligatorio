
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLogs.LogsStorage.GameLogs;
using ServerLogs.Models;

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
        public async Task<ActionResult<IEnumerable<GameLogModel>>> GetGameLogs()
        {
            return _context.GetLogs();
        }

        [HttpPost]
        public ActionResult<List<GameLogModel>> GetGameLogs(FilterModel filters)
        {
            var gameLog = _context.GetLogs(filters);
            if (gameLog == null)
            {
                return NotFound();
            }

            return gameLog;
        }

        /*[HttpPost]
        public async Task<ActionResult<GameLogModel>> PostGameLogs(GameLogModel gameLog)
        {
            _context.AddGameLog(gameLog);

            return CreatedAtAction(nameof(GetGameLogs), new { id = gameLog.Id }, gameLog);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameLogModel>> DeleteGameLog(int id)
        {
            var gameLog = _context.DeleteLog(id);
            if (gameLog == null)
            {
                return NotFound();
            }
            return gameLog;
        }*/
    }
}
