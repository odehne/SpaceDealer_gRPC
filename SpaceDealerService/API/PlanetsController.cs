using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceDealerModels.Units;
using System.Threading.Tasks;

namespace SpaceDealerService.API
{
    [ApiController]
    [Produces("application/json")]
    public class PlanetsController : Controller
    {
        /// <summary>
        /// Returns all planets in the game.
        /// </summary>
        /// <returns>Planets</returns>
        /// <response code="200">Planets found.</response>
        [HttpGet]
        [Route("planets/")]
        [ProducesResponseType(typeof(Planets), StatusCodes.Status200OK)]
        public async Task<ActionResult<Planets>> GetAll()
        {
            return Ok(Program.TheGame.Galaxy);
        }

        /// <summary>
        /// Returns the planet with the corresponding planet UID.
        /// </summary>
        /// <param name="planetUID">Planet UID</param>
        /// <returns>DbPlanet</returns>
        /// <response code="200">Planet found.</response>
        /// <response code="404">Planet not found.</response>
        [HttpGet]
        [Route("planets/{planetUID}")]
        [ProducesResponseType(typeof(DbPlanet), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbPlanet>> GetPlanet([FromRoute] string planetUID)
        {
            var planet = Program.TheGame.Galaxy.GetPlanetById(planetUID);

            if (planet == null)
                return NotFound($"Planet with id [{planetUID}] not found.");

            return Ok(planet);
        }

        /// <summary>
        /// Returns all planets known to the specified player.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <returns>Planets</returns>
        /// <response code="200">Planets found.</response>
        [HttpGet]
        [Route("planets/discovered/{playerUID}")]
        [ProducesResponseType(typeof(Planets), StatusCodes.Status200OK)]
        public async Task<ActionResult<Planets>> GetAll([FromRoute] string playerUID)
        {
            var player = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);

            if (player == null)
                return NotFound($"Player with id [{playerUID}] not found.");

            var discoveredPlanets = player.DiscoveredPlanets;

            return Ok(discoveredPlanets);
        }

      
    }
}
