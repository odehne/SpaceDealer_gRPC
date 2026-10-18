using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceDealerModels.Units;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SpaceDealerService.API
{

    [ApiController]
    [Route("game")]
    [Produces("application/json")]
    public class GameController : Controller
    {
       
        [HttpGet]
        [Route("{gameUID}/planets")]
        [ProducesResponseType(typeof(Planets), StatusCodes.Status200OK)]
        public async Task<ActionResult<Planets>> GetallPlanets([FromRoute] string gameUID)
        {
            return Program.TheGame.Galaxy;
        }

        [HttpGet]
        [Route("{gameUID}/players")]
        [ProducesResponseType(typeof(Players), StatusCodes.Status200OK)]
        public async Task<ActionResult<Players>> GetallPlayers([FromRoute] string gameUID)
        {
            return Program.TheGame.FleetCommanders;
        }

        [HttpPost]
        [Route("save")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> SaveGame(string playerName)
        {
            try
            {
                Program.Persistor.SaveGalaxy(Program.TheGame.Galaxy);
                Program.Persistor.SavePlayers(Program.TheGame.FleetCommanders);
                return Ok(true);
            }
            catch (Exception e)
            {
                Program.TheLogger.Log($"Failed to save game ({e.Message}).", TraceEventType.Warning);
                return BadRequest("Failed to save game.");
            }
        }
    }
}
