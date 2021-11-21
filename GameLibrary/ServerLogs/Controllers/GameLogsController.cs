using System.Collections.Generic;
using System.Threading.Tasks;
using LogsModels;
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
        public ActionResult<List<LogGameModel>> GetGameLogs([FromQuery] FilterModel filters)
        {
            var gameLog = _context.GetLogs(filters);
            if (gameLog == null)
            {
                return NotFound();
            }

            return gameLog;
        }

    }
}