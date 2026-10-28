using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SpaceDealer;
using SpaceDealerModels.Units;
using System;
using System.Threading.Tasks;

namespace SpaceDealerService.API
{
    [ApiController]
    [Route("game")]
    [Produces("application/json")]
    public class GameController : Controller
    {

        private readonly ILogger _logger;


        /// <summary>
        /// Load a saved game by game UID.
        /// </summary>
        /// <param name="gameUID">Game UID</param>
        /// <returns>Game UID</returns>
        /// <response code="200">Game found.</response>
        /// <response code="404">Game not found.</response>
        [HttpGet]
        [Route("game/load")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> LoadGame(string gameUID)
        {
            try
            {
                Program.TheGame.LoadGame(gameUID);
                return Ok(Program.TheGame.GameId);
            }
            catch (Exception e)
            {
                _logger.Warning($"Failed to load game ({e.Message}).");
                return NotFound("Failed to load game with the specified UID.");
            }
        }

        /// <summary>
        /// Get statistics of the current game by game UID.
        /// </summary>
        /// <param name="gameUID">Game UID</param>
        /// <returns>Game UID</returns>
        /// <response code="200">Game found.</response>
        /// <response code="404">Game not found.</response>
        [HttpGet]
        [Route("game/stats")]
        [ProducesResponseType(typeof(GameStatistics), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> GetGameStats(string gameUID)
        {
            try
            {
                var stats = Program.TheGame.GetGameStats();
                return Ok(stats);
            }
            catch (Exception e)
            {
                _logger.Warning($"Failed to collect game statistics ({e.Message}).");
                return NotFound("Failed to collect game statistics with the specified UID.");
            }
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        /// <returns>Game UID</returns>
        /// <response code="200">Game found.</response>
        /// <response code="400">Failed to start game.</response>
        [HttpPost]
        [Route("game/new")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> NewGame()
        {
            try
            {
                Program.TheGame.StartNewGame();
                return Ok(Program.TheGame.GameId);
            }
            catch (Exception e)
            {
                _logger.Warning($"Failed to start game ({e.Message}).");
                return BadRequest("Failed to start game.");
            }
        }

        /// <summary>
        /// Save the current game by game UID.
        /// </summary>
        /// <param name="gameUID">Game UID</param>
        /// <returns>Game UID</returns>
        /// <response code="200">True</response>
        /// <response code="400">Failed to save game.</response>
        [HttpPut]
        [Route("game/save")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> SaveGame(string gameUID)
        {
            try
            {
                Program.TheGame.SaveGalaxy();
                Program.TheGame.SavePlayers();
                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.Warning($"Failed to save game ({e.Message}).");
                return BadRequest("Failed to save game.");
            }
        }

        /// <summary>
        /// Reset Database and start a new game.
        /// </summary>
        /// <param name="gameUID">Game UID</param>
        /// <returns>Game UID</returns>
        /// <response code="200">True</response>
        /// <response code="400">Failed to save game.</response>
        [HttpPut]
        [Route("game/reset")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> ResetDatabase()
        {
            try
            {
                Program.TheGame.ResetDatabase();
                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.Warning($"Failed to reset database ({e.Message}).");
                return BadRequest("Failed to reset database.");
            }
        }
    }
}
