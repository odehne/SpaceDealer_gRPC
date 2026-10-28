using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SpaceDealerModels.Repositories;
using SpaceDealerModels.Units;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceDealerService.API
{
    [ApiController]
    [Produces("application/json")]
    public class PlayersController : Controller
    {

        private readonly ILogger _logger;

        /// <summary>
        /// Returns all players in the game.
        /// </summary>
        /// <returns>DbPlayer</returns>
        /// <response code="200">Player found.</response>
        /// <response code="404">Player not found.</response>
        [HttpGet]
        [Route("players")]
        [ProducesResponseType(typeof(DbPlayer), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbPlayer>> GetAll()
        {
          
            if(Program.TheGame.FleetCommanders.Count > 0)
                return Ok(Program.TheGame.FleetCommanders);

            return NotFound("No players found.");
        }

        /// <summary>
        /// Returns the player with the corresponding player UID.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <returns>DbPlayer</returns>
        /// <response code="200">Player found.</response>
        /// <response code="404">Player not found.</response>
        [HttpGet]
        [Route("players/{playerUID}")]
        [ProducesResponseType(typeof(DbPlayer), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbPlayer>> GetPlayer([FromRoute] string playerUID)
        {
            if(string.IsNullOrEmpty(playerUID))
                return BadRequest("Player UID is required.");

            var player = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);

            if (player != null)
                return Ok(player);

            return NotFound($"Player with id [{playerUID}] not found.");
        }

        /// <summary>
        /// Returns the player with the corresponding player UID.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <returns>DbPlayer</returns>
        /// <response code="200">Player found.</response>
        /// <response code="404">Player not found.</response>
        [HttpGet]
        [Route("players/search")]
        [ProducesResponseType(typeof(DbPlayer), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbPlayer>> GetPlayerByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Player search value is required.");

            var player = Program.TheGame.FleetCommanders.GetPlayerByName(name);

            if (player != null)
                return Ok(player);

            return NotFound($"Player with name [{name}] not found.");
        }


        /// <summary>
        /// Returns any updates for the player with the corresponding player UID.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <returns>UpdateInfo</returns>
        /// <response code="200">The current list of infos or an empry list if nothing happened.</response>
        [HttpGet]
        [Route("players/{playerUID}/updates")]
        [ProducesResponseType(typeof(UpdateInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<UpdateInfo>> GetUpdates([FromRoute] string playerUID)
        {
            var p = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);
            if (p == null)
                return NotFound($"Player with id [{playerUID}] not found.");

            if (p.UpdateQueue != null)
            {
                while (p.UpdateQueue.Count > 0)
                {
                    var info = (UpdateInfo)p.UpdateQueue.Dequeue();
                    return Ok(info);
                }
            }
            return Ok(new UpdateInfo());
        }

        /// <summary>
        /// Returns the list of ships for the player with the corresponding player UID.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <returns>Ships</returns>
        /// <response code="200">List of ships of the player.</response>
        /// <response code="404">Player not found.</response>
        [HttpGet]
        [Route("players/{playerUID}/ships")]
        [ProducesResponseType(typeof(Ships), StatusCodes.Status200OK)]
        public async Task<ActionResult<Ships>> GetAllShips([FromRoute] string playerUID)
        {
            var p = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);

            if (p == null)
                return NotFound(false);

            return Ok(p.Fleet);
        }

        /// <summary>
        /// Returns the ship for the player with the corresponding player UID and ship UID.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID</param>
        /// <returns>Ships</returns>
        /// <response code="200">A ship.</response>
        /// <response code="404">Ship or Player not found.</response>
        [HttpGet]
        [Route("players/{playerUID}/ships/{shipUID}")]
        [ProducesResponseType(typeof(DbShip), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbShip>> GetShip([FromRoute] string playerUID, [FromRoute] string shipUID)
        {
            var p = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);

            if (p == null)
                return NotFound($"Player [{playerUID}] not found.");

            var ship = p.Fleet.GetShipById(shipUID);

            if (ship == null)
                return NotFound($"Ship [{shipUID}] not found.");

            return Ok(ship);
        }

        /// <summary>
        /// Returns the ships of other pilots that are inside the scan range of the given ship.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID</param>
        /// <response code="200">Ships of other pilots in range.</response>
        /// <response code="404">Ship or Player not found.</response>
        [HttpGet]
        [Route("players/{playerUID}/ships/{shipUID}/nearby")]
        public async Task<ActionResult> GetNearbyShips([FromRoute] string playerUID, [FromRoute] string shipUID)
        {
            var p = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);

            if (p == null)
                return NotFound($"Player [{playerUID}] not found.");

            var ship = p.Fleet.GetShipById(shipUID);

            if (ship?.Cruise?.CurrentSector == null)
                return NotFound($"Ship [{shipUID}] not found.");

            // Same cube as DbJourney.ScanPlanetsInNearbySectors: 1 + RangeBonus sectors in every direction.
            var layer = 1 + ship.RangeBonus;
            var origin = ship.Cruise.CurrentSector;

            var nearby = Program.TheGame.FleetCommanders.Values
                .Where(other => other.Id != p.Id)
                .SelectMany(other => other.Fleet.Values.Select(s => new { Pilot = other, Ship = s }))
                .Where(x => x.Ship.Cruise?.CurrentSector != null
                    && Math.Abs(x.Ship.Cruise.CurrentSector.X - origin.X) <= layer
                    && Math.Abs(x.Ship.Cruise.CurrentSector.Y - origin.Y) <= layer
                    && Math.Abs(x.Ship.Cruise.CurrentSector.Z - origin.Z) <= layer)
                .Select(x => new
                {
                    shipId = x.Ship.Id,
                    shipName = x.Ship.Name,
                    pilotName = x.Pilot.Name,
                    sector = x.Ship.Cruise.CurrentSector
                })
                .ToList();

            return Ok(nearby);
        }

        /// <summary>
        /// Repairs the ship for the player with the corresponding player UID and ship UID.
        /// If the hull needs a repair, 1 repair action costs 5.000 Credits. 
        /// If the hull is at max, the shields can be repaired. 1 shield repair, 5.000 Credits.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID</param>
        /// <returns>Ships</returns>
        /// <response code="200">A ship.</response>
        /// <response code="404">Ship or Player not found.</response>
        /// <response code="400">Repair not possible.</response>
        [HttpPut]
        [Route("players/{playerUID}/ships/{shipUID}/repair")]
        [ProducesResponseType(typeof(DbShip), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbShip>> Repair([FromRoute] string playerUID, [FromRoute] string shipUID)
        {
            var p = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);

            if (p == null)
                return NotFound($"Player [{playerUID}] not found.");

            var ship = p.Fleet.GetShipById(shipUID);

            if (ship == null)
                return NotFound($"Ship [{shipUID}] not found.");

            if(ship.Hull < DbShip.DEFAULT_MAX_HULL_MID_SIZE_FREIGHTER)
            {
                if (p.Credits - 5000 > 0)
                {
                    p.Credits -= 5000;
                    ship.Hull += 1;
                }
            }
            else
            {
                if (ship.Shields < DbShip.DEFAULT_MAX_SHIELDS_MID_SIZE_FREIGHTER)
                {
                    if (p.Credits - 5000 > 0)
                    {
                        p.Credits -= 5000;
                        ship.Shields += 1;
                    }
                }
            }
            
            return Ok(ship);
        }

        /// <summary>
        /// Returns the ship for the player with the corresponding player UID and ship UID.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID</param>
        /// <returns>Ships</returns>
        /// <response code="200">A ship.</response>
        /// <response code="404">Ship or Player not found.</response>
        [HttpPut]
        [Route("players/{playerUID}/ships/{shipUID}")]
        [ProducesResponseType(typeof(DbShip), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbShip>> SendProbe([FromRoute] string playerUID, [FromRoute] string shipUID)
        {
            var p = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);

            if (p == null)
                return NotFound($"Player [{playerUID}] not found.");

            var ship = p.Fleet.GetShipById(shipUID);

            if (ship == null)
                return NotFound($"Ship [{shipUID}] not found.");

            var probe = ship.Probes.Values.FirstOrDefault(c => c.State == ProbeStates.InActive);

            if (probe == null)
                return BadRequest("No probe available to send into space.");

            probe.Eject();

            return Ok(ship);
        }

        /// <summary>
        /// Creates a new player by name.
        /// </summary>
        /// <param name="playerName">Name of the new player.</param>
        /// <returns>Ships</returns>
        /// <response code="200">A ship.</response>
        /// <response code="400">Ship or Player not found.</response>
        [HttpPost]
        [Route("players/new")]
        [ProducesResponseType(typeof(DbPlayer), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbPlayer>> NewPlayer([FromQuery] string playerName)
        {
            var player = Program.TheGame.GetPlayerByName(playerName);

            if (player == null)
            {
                var homePlanet = Program.TheGame.Galaxy.GetRandomPlanet();
                player = new DbPlayer(playerName, homePlanet, Program.TheGame.Galaxy.GetFirstFivePlanets(), Program.TheGame.Game)
                {
                    PlayerType = SpaceDealer.Enums.PlayerTypes.Real
                };
                //player.PicturePath = request.PicturePath;
            }
            else
            { 
                player.Galaxy = Program.TheGame.Game.Galaxy;
                player.ActiveSectors = Program.TheGame.Game.KnownUniverse;
            }
            var p = Program.TheGame.Game.FleetCommanders.AddPlayer(player);

            try
            {
                Program.TheGame.SavePlayer(p);
            }
            catch (Exception e)
            {
                _logger.Warning($"Failed to add new player ({e.Message}).");
                return BadRequest("Failed to add new player.");
            }
            return p;
        }


        /// <summary>
        /// Creates a new ship for the player by name.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipName">Name of the new player.</param>
        /// <returns>Ships</returns>
        /// <response code="200">A ship.</response>
        /// <response code="400">Ship name not specified or failed to save.</response>
        /// <response code="404">Player not found.</response>
        [HttpPost]
        [Route("players/{playerUID}/ships/new")]
        [ProducesResponseType(typeof(DbShip), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbShip>> NewShip([FromRoute] string playerUID, [FromQuery] string shipName)
        {

            if (string.IsNullOrEmpty(shipName))
                return BadRequest("");

            var player = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);

            if(player== null)
                return NotFound($"Player with UID [{playerUID}] not found.");

            // let's see if the player has already a ship registered with this name
            var s = player.Fleet.GetShipByName(shipName);
            if (s != null)
                return BadRequest($"The player has already a ship with the name [{shipName}].");

            if (player.Credits < DbShip.NEW_SHIP_COSTS)
                return BadRequest($"Ein neues Schiff kostet {DbShip.NEW_SHIP_COSTS:N0} Credits.");

            s = new DbShip(shipName,
                               player,
                               player.Parent.Features.GetFeatureSet([FeatureNames.SignalRange_1]), player.Fleet)
            {
                PicturePath = ".\\Spaceships\\MediumFrighter.jpg"
            };

            try
            {
                player.Fleet.AddShip(s);
                player.Credits -= DbShip.NEW_SHIP_COSTS;
                Program.TheGame.SavePlayers();
            }
            catch (Exception e)
            {
                _logger.Warning($"Failed to add new ship ({e.Message}) for player [{player.Name}].");
                return BadRequest("Failed to add new ship.");
            }

            return s;
        }

        /// <summary>
        /// Attacks another ship.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID.</param>
        /// <returns>BattleResult</returns>
        /// <response code="200">Attack done.</response>
        /// <response code="400">Ship name not specified or failed to save.</response>
        /// <response code="404">Player not found.</response>
        [HttpPost]
        [Route("players/{playerUID}/ships/{shipUID}/attack")]
        [ProducesResponseType(typeof(BattleResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<BattleResult>> BattleAttack([FromRoute] string playerUID, [FromRoute] string shipUID)
        {
            var p = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);
            if (p == null)
                return NotFound($"Player with UID [{playerUID}] not found.");
            var s = p.Fleet.GetShipById(shipUID);
            if (s == null)
                return NotFound($"Ship with UID [{shipUID}] not found.");

            try
            {
                var result = s.Attack();
                if (result.Defeaded == true)
                {
                    p.Credits += result.Treasure;
                    s.Cruise.ContinueTravel();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Warning($"Failed to apply attack for player [{p.Name} - {s.Name}].");
                return BadRequest("Failed to apply attack for player.");
            }


        }

        /// <summary>
        /// Defends an attack.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID.</param>
        /// <returns>BattleResult</returns>
        /// <response code="200">Defence manouvre done.</response>
        /// <response code="400">Ship name not specified or failed to save.</response>
        /// <response code="404">Player not found.</response>
        [HttpPost]
        [Route("players/{playerUID}/ships/{shipUID}/defend")]
        [ProducesResponseType(typeof(BattleResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<BattleResult>> BattleDefend([FromRoute] string playerUID, [FromRoute] string shipUID)
        {
            var p = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);
            if (p == null)
                return NotFound($"Player with UID [{playerUID}] not found.");
            var s = p.Fleet.GetShipById(shipUID);
            if (s == null)
                return NotFound($"Ship with UID [{shipUID}] not found.");

            try
            {
                var result = s.Defend();
                //TODO: What shall we do here?
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Warning($"Failed to apply defence for player [{p.Name} - {s.Name}].");
                return BadRequest("Failed to apply defence for player.");
            }
        }

        /// <summary>
        /// Starts the travel of a ship to either a planet or coordinates.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID.</param>
        /// <returns>BattleResult</returns>
        /// <response code="200">Travel started.</response>
        /// <response code="400">Ship name not specified or failed to save.</response>
        /// <response code="404">Player not found.</response>
        [HttpPost]
        [HttpPut]
        [Route("players/{playerUID}/ships/{shipUID}/travel/to")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> StartCruise([FromRoute] string playerUID, [FromRoute] string shipUID, [FromQuery] string destinationPlanetUID = "", [FromBody] DbCoordinates newPosition = null)
        {
            var p = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);
            if (p == null)
                return NotFound($"Player with [{playerUID}] not found.");

            var s = p.Fleet.GetShipById(shipUID);
            if (s == null)
                return NotFound($"Ship with [{shipUID}] not found.");

            if (!string.IsNullOrEmpty(destinationPlanetUID))
            {

                var planet = Program.TheGame.Galaxy.GetPlanetById(destinationPlanetUID);
                if (planet == null)
                    return NotFound($"Planet with [{destinationPlanetUID}] not found.");

                try
                {
                    s.StartCruise(s.CurrentPlanet, planet);
                    return Ok(true);
                }
                catch (Exception e)
                {
                    _logger.Warning($"Failed to start cruise for player [{p.Name} - {s.Name}]. Error: {e.Message}");
                    return BadRequest("Failed to start cruise.");
                }
            }

            if (newPosition != null)
            {
                try
                {
                    return BadRequest($"Cruising to destination coordinates is not supported.");

                    //s.StartCruiseToLocation(s.Cruise.CurrentSector, new DbCoordinates(newPosition.X, newPosition.Y, newPosition.Z));
                    //return Ok(true);
                }
                catch (Exception e)
                {
                    _logger.Warning($"Failed to start cruise to coordinates {newPosition} for player [{p.Name} - {s.Name}]. Error: {e.Message}");
                    return BadRequest("Failed to start cruise to coordinates.");
                }
            }

            return BadRequest($"Missing planetUID or destination coordinates.");

        }

        /// <summary>
        /// Continues a travel of a ship.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID.</param>
        /// <returns>BattleResult</returns>
        /// <response code="200">Travel contninues.</response>
        /// <response code="400">Ship name not specified or failed to save.</response>
        /// <response code="404">Player not found.</response>
        [HttpPut]
        [Route("players/{playerUID}/ships/{shipUID}/travel/continue")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> ContinueTravel([FromRoute] string playerUID, [FromRoute] string shipUID, [FromBody] DbCoordinates newPosition)
        {
            var player = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);
            if (player == null)
                return NotFound($"Player with [{playerUID}] not found.");

            var ship = player.Fleet.GetShipById(shipUID);
            if (ship == null)
                return NotFound($"Ship with [{shipUID}] not found.");

            try
            {
                ship.Cruise.ContinueTravel();
                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.Warning($"Failed to start cruise to coordinates {newPosition} for player [{player.Name} - {ship.Name}].");
                return BadRequest("Failed to start cruise to coordinates.");
            }
        }

    }
}
