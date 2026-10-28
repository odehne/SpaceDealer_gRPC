using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SpaceDealerModels.Units;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace SpaceDealerService.API
{
    [ApiController]
    [Route("trading")]
    [Produces("application/json")]
    public class TradingController : Controller
    {
        private readonly ILogger _logger;

        /// <summary>Share of the cargo's market price paid when the planet has no demand for it.</summary>
        public const double DUMP_PRICE_FACTOR = 0.6;

        /// <summary>
        /// Returns the needed products for a specific planet.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID</param>
        /// <param name="planetUID">Planet UID</param>
        /// <returns>DbIndustry</returns>
        /// <response code="200">Needed products found.</response>
        /// <response code="404">Needed products not found.</response>
        [HttpGet]
        [Route("trading/{playerUID}/{shipUID}/{planetUID}/needs")]
        [ProducesResponseType(typeof(DbIndustry), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbIndustry>> GetNeededProducts([FromRoute] string playerUID, [FromRoute] string shipUID, [FromRoute] string planetUID)
        {
            var player = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);
            var ship = player?.Fleet.GetShipById(shipUID);
            var planet = Program.TheGame.Galaxy.GetPlanetById(planetUID);

            if (player == null || ship == null || planet == null)
                return NotFound("One or more requested resources not found.");

            return Ok(planet.Industry?.ProductsNeeded);
        }

        /// <summary>
        /// Returns the needed products for a specific planet.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID</param>
        /// <param name="planetUID">Planet UID</param>
        /// <param name="productUID">Product UID</param>
        /// <returns>DbIndustry</returns>
        /// <response code="200">Needed products found.</response>
        /// <response code="404">Needed products not found.</response>
        [HttpGet]
        [Route("trading/{playerUID}/{shipUID}/{planetUID}/{productUID}/sell")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        public async Task<ActionResult<double>> SellProducts([FromRoute] string playerUID, [FromRoute] string shipUID, [FromRoute] string planetUID, [FromRoute] string productUID, [FromQuery] double quantity)
        {
            var player = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);
            var ship = player?.Fleet.GetShipById(shipUID);
            var planet = Program.TheGame.Galaxy.GetPlanetById(planetUID);
            var product = planet?.Industry?.ProductsNeeded?.FirstOrDefault(p => p.Id == productUID);

            if (player == null || ship == null || planet == null)
                return NotFound("One or more requested resources not found.");

            if(quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            var load = ship.CargoBay?.GetCargoByProductId(productUID);
            if (load == null || load.Quantity < quantity)
                return BadRequest("Not enough product in ship's cargo bay.");

            // Nobody here needs the product: the cargo can still be dumped, but only at a discount.
            var pricePerTon = product != null
                ? product.PricePerTon
                : load.Product.PricePerTon * DUMP_PRICE_FACTOR;

            ship.CargoBay?.RemoveCargo(productUID, quantity);

            var totalPrice = pricePerTon * quantity;
            
            player.Credits += totalPrice;

            return Ok(totalPrice);
        }

        /// <summary>
        /// Returns the needed products for a specific planet.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID</param>
        /// <param name="planetUID">Planet UID</param>
        /// <param name="productUID">Product UID</param>
        /// <returns>DbIndustry</returns>
        /// <response code="200">Needed products found.</response>
        /// <response code="404">Needed products not found.</response>
        [HttpGet]
        [Route("trading/{playerUID}/{shipUID}/{planetUID}/{productUID}/buy")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        public async Task<ActionResult<double>> BuyProducts([FromRoute] string playerUID, [FromRoute] string shipUID, [FromRoute] string planetUID, [FromRoute] string productUID, [FromQuery] double quantity)
        {
            var player = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);
            var ship = player?.Fleet.GetShipById(shipUID);
            var planet = Program.TheGame.Galaxy.GetPlanetById(planetUID);
            var product = planet?.Industry?.GeneratedProducts?.FirstOrDefault(p => p.Id == productUID);

            if (player == null || ship == null || planet == null || product == null)
                return NotFound("One or more requested resources not found.");

            if (quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            if (ship.CargoSize - ship.CargoBay?.TotalQuantity < quantity)
                return BadRequest("Not enough space in ship's cargo bay.");

            ship.CargoBay?.AddCargo(ship, product, quantity);

            var totalPrice = product.PricePerTon * quantity;

            player.Credits -= totalPrice;

            planet?.Industry?.GeneratedProducts?.FirstOrDefault(p => p.Id == productUID)?.Amount -= quantity;

            return Ok(totalPrice);
        }

        public enum TradeType
        {
            Buy,
            Sell
        }

        /// <summary>
        /// Returns all known planets that sell or buy the specified product.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID</param>
        /// <param name="productUID">Product UID</param>
        /// <returns>DbIndustry</returns>
        /// <response code="200">Needed products found.</response>
        /// <response code="404">Needed products not found.</response>
        [HttpGet]
        [Route("trading/{playerUID}/{shipUID}/browse")]
        [ProducesResponseType(typeof(DbPlanet), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbPlanet>> BrowseBestPrice([FromRoute] string playerUID, [FromRoute] string shipUID, [FromQuery] string productUID, [FromQuery] TradeType tradeType)
        {
            var player = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);
            var ship = player?.Fleet.GetShipById(shipUID);
            
            if (player == null || ship == null )
                return NotFound("One or more requested resources not found.");


            var planets = player.DiscoveredPlanets.Values;

            DbPlanet? bestPrice = tradeType switch
            {
                // Planets that need the product: the highest price is best for the player selling there
                TradeType.Buy => planets
                    .Where(p => p.Industry?.ProductsNeeded?.Any(x => x.Id == productUID) == true)
                    .MaxBy(p => p.Industry!.ProductsNeeded.First(x => x.Id == productUID).PricePerTon),

                // Planets that produce the product: the lowest price is best for the player buying there
                TradeType.Sell => planets
                    .Where(p => p.Industry?.GeneratedProducts?.Any(x => x.Id == productUID) == true)
                    .MinBy(p => p.Industry!.GeneratedProducts.First(x => x.Id == productUID).PricePerTon),

                _ => null
            };

            if(bestPrice != null)
                return Ok(bestPrice);

            return NotFound("Product not found on any discovered planets.");
        }

        /// <summary>
        /// Returns the produced products for a specific planet.
        /// </summary>
        /// <param name="playerUID">Player UID</param>
        /// <param name="shipUID">Ship UID</param>
        /// <param name="planetUID">Planet UID</param>
        /// <returns>DbIndustry</returns>
        /// <response code="200">Produced products found.</response>
        /// <response code="404">Produced products not found.</response>
        [HttpGet]
        [Route("trading/{playerUID}/{shipUID}/{planetUID}/produced")]
        [ProducesResponseType(typeof(DbIndustry), StatusCodes.Status200OK)]
        public async Task<ActionResult<DbIndustry>> GetProducedProducts([FromRoute] string playerUID, [FromRoute] string shipUID, [FromRoute] string planetUID)
        {
            var player = Program.TheGame.FleetCommanders.GetPlayerById(playerUID);
            var ship = player?.Fleet.GetShipById(shipUID);
            var planet = Program.TheGame.Galaxy.GetPlanetById(planetUID);

            if (player == null || ship == null || planet == null)
                return NotFound("One or more requested resources not found.");

            return Ok(planet.Industry?.GeneratedProducts);
        }
    }
}
