using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SpaceDealerModels.Units;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceDealerService.API
{
    [ApiController]
    [Produces("application/json")]
    public class ShipyardController : Controller
    {
        private readonly ILogger _logger = Log.Logger;

        /// <summary>
        /// Returns all features that can be bought at the shipyard.
        /// </summary>
        /// <response code="200">The feature catalog.</response>
        [HttpGet]
        [Route("shipyard/features")]
        [ProducesResponseType(typeof(DbFeature[]), StatusCodes.Status200OK)]
        public ActionResult<DbFeature[]> GetFeatures()
        {
            return Ok(Program.TheGame.Game.Features.Values.OrderBy(f => f.Costs).ToArray());
        }

        /// <summary>
        /// Buys a feature for a ship.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID</param>
        /// <param name="featureUID">Feature UID</param>
        /// <returns>The upgraded ship.</returns>
        /// <response code="200">Feature installed.</response>
        /// <response code="400">Not enough credits or feature already installed.</response>
        /// <response code="404">Player, ship or feature not found.</response>
        [HttpPost]
        [Route("players/{playerUID}/ships/{shipUID}/features/{featureUID}/buy")]
        [ProducesResponseType(typeof(DbShip), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbShip>> BuyFeature([FromRoute] string playerUID, [FromRoute] string shipUID, [FromRoute] string featureUID)
        {
            var player = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);
            if (player == null)
                return NotFound($"Player [{playerUID}] not found.");

            var ship = player.Fleet.GetShipById(shipUID);
            if (ship == null)
                return NotFound($"Ship [{shipUID}] not found.");

            var feature = Program.TheGame.Game.Features.GetFeatureById(featureUID);
            if (feature == null)
                return NotFound($"Feature [{featureUID}] not found.");

            var isProbe = feature.Name.Equals(nameof(FeatureNames.AdditionalProbe), StringComparison.OrdinalIgnoreCase);
            if (!isProbe && ship.Features.ContainsKey(feature.Id))
                return BadRequest("Dieses Feature ist bereits eingebaut.");

            if (player.Credits < feature.Costs)
                return BadRequest($"Nicht genügend Credits ({feature.Costs:N0} benötigt).");

            try
            {
                ship.InstallFeature(feature);
                player.Credits -= feature.Costs;
                Program.TheGame.SavePlayers();
            }
            catch (Exception e)
            {
                _logger.Warning($"Failed to install feature [{feature.Name}] on ship [{ship.Name}]: {e.Message}");
                return BadRequest("Failed to install feature.");
            }

            return Ok(ship);
        }
    }
}
