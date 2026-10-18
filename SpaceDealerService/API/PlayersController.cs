using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        /// <summary>
        /// Returns the player with the corresponding player UID.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <returns>DbPlayer</returns>
        /// <response code="200">Player found.</response>
        /// <response code="404">Player not found.</response>
        [HttpGet]
        [Route("{playerUID}")]
        [ProducesResponseType(typeof(DbPlayer), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbPlayer>> GetPlayer([FromRoute] string playerUID)
        {
            var p = Program.TheGame.FleetCommanders.FirstOrDefault(x => x.Name == playerUID);

            if (p != null)
                return Ok(p);

            return NotFound($"Player with id [{playerUID}] not found.");
        }


        /// <summary>
        /// Returns any updates for the player with the corresponding player UID.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <returns>UpdateInfo</returns>
        /// <response code="200">The current list of infos or an empry list if nothing happened.</response>
        [HttpGet]
        [Route("{playerUID}/updates")]
        [ProducesResponseType(typeof(UpdateInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<UpdateInfo>> GetUpdates([FromRoute] string playerUID)
        {
            var player = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);

            if (player.UpdateQueue != null)
            {
                while (player.UpdateQueue.Count > 0)
                {
                    var info = (UpdateInfo)player.UpdateQueue.Dequeue();
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
        [Route("{playerUID}/ships")]
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
        [Route("player/{playerUID}/ships/{shipUID}")]
        [ProducesResponseType(typeof(DbShip), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbShip>> GetShip([FromRoute] string playerUID, [FromRoute] string shipUID)
        {
            var p = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);

            if (p == null)
                return NotFound($"Player [{playerUID}] not found.");

            var ship = p.Fleet.FirstOrDefault(x => x.Id == shipUID);

            if (ship == null)
                return NotFound($"Ship [{shipUID}] not found.");

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
        [Route("player/new")]
        [ProducesResponseType(typeof(DbPlayer), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbPlayer>> NewPlayer([FromQuery] string playerName)
        {
            var player = Program.Persistor.PlayersRepo.GetItem(playerName, null);

            if (player == null)
            {
                var homePlanet = Program.TheGame.Galaxy.GetRandomPlanet();
                player = new DbPlayer(playerName, homePlanet, Program.TheGame.Galaxy.GetFirstFivePlanets(), Program.TheGame.Galaxy, Program.TheGame.ActiveSectors)
                {
                    PlayerType = SpaceDealer.Enums.PlayerTypes.Real
                };
                //player.PicturePath = request.PicturePath;
            }
            else
            {
                player.Galaxy = Program.TheGame.Galaxy;
                player.ActiveSectors = Program.TheGame.ActiveSectors;
            }
            var p = Program.TheGame.FleetCommanders.AddPlayer(player);

            try
            {
                Program.Persistor.SavePlayers(Program.TheGame.FleetCommanders);
            }
            catch (Exception e)
            {
                Program.TheLogger.Log($"Failed to add new player ({e.Message}).", TraceEventType.Warning);
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
        [Route("player/{playerUID}/ships/new")]
        [ProducesResponseType(typeof(DbShip), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbShip>> NewShip([FromRoute] string playerUID, [FromQuery] string shipName)
        {

            if (string.IsNullOrEmpty(shipName))
                return BadRequest("");

            var p = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);

            if(p== null)
                return NotFound($"Player with UID [{playerUID}] not found.");

            // let's see if the player has already a ship registered with this name
            var s = p.Fleet.GetShipByName(shipName);
            if (s != null)
                return BadRequest($"The player has already a ship with the name [{shipName}].");

            s = new DbShip(shipName,
                               p.HomePlanet,
                               Repository.GetFeatureSet(new string[] { "SignalRange+1" }), p.Fleet)
            {
                CargoSize = 30,
                PlayerId = p.Id,
                PicturePath = ".\\Spaceships\\MediumFrighter.jpg"
            };

            try
            {
                p.Fleet.AddShip(s);
                Program.Persistor.SavePlayers(Program.TheGame.FleetCommanders);
            }
            catch (Exception e)
            {
                Program.TheLogger.Log($"Failed to add new ship ({e.Message}) for player [{p.Name}].", TraceEventType.Warning);
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
        [Route("player/{playerUID}/ships/{shipUID}/attack")]
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
                    p.Credits += result.Treasure;
                return Ok(result);
            }
            catch (Exception e)
            {
                Program.TheLogger.Log($"Failed to apply attack for player [{p.Name} - {s.Name}].", TraceEventType.Warning);
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
        [Route("player/{playerUID}/ships/{shipUID}/defend")]
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
                Program.TheLogger.Log($"Failed to apply defence for player [{p.Name} - {s.Name}].", TraceEventType.Warning);
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
        [Route("player/{playerUID}/ships/{shipUID}/travel/to")]
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
                    Program.TheLogger.Log($"Failed to start cruise for player [{p.Name} - {s.Name}].", TraceEventType.Warning);
                    return BadRequest("Failed to start cruise.");
                }
            }

            if (newPosition != null)
            {
                try
                {
                    s.StartCruiseToLocation(s.Cruise.CurrentSector, new DbCoordinates(newPosition.X, newPosition.Y, newPosition.Z));
                    return Ok(true);
                }
                catch (Exception e)
                {
                    Program.TheLogger.Log($"Failed to start cruise to coordinates {newPosition} for player [{p.Name} - {s.Name}].", TraceEventType.Warning);
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
        [Route("player/{playerUID}/ships/{shipUID}/travel/continue")]
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
                Program.TheLogger.Log($"Failed to start cruise to coordinates {newPosition} for player [{player.Name} - {ship.Name}].", TraceEventType.Warning);
                return BadRequest("Failed to start cruise to coordinates.");
            }
        }

    }
}
