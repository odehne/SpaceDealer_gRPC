using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using Galaxy.Creator.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Controllers
{

	[Route("api/v1/[controller]")]
	[ApiController]
	public class PlanetController : ControllerBase
	{
        private readonly IMediator _mediator;
        private readonly ILogger<PlanetController> _logger;
        private readonly IPlanetRepository _repo;

		public PlanetController(IMediator mediator, ILogger<PlanetController> logger, IPlanetRepository repo)
		{
			_mediator = mediator;
			_logger = logger;
			_repo = repo;
		}
    
        [HttpGet]
        public async Task<IEnumerable<PlanetDto>> Get()
        {
            return await _mediator.Send(new PlanetsQuery());
        }
        // GET api/v1/[controller]/id]
        [HttpGet]
        [Route("{id}")]
        public async Task<PlanetDto> GetPlanet(string id)
        {
            return await _mediator.Send(new PlanetQuery(id));
        }

        

        //[Route("productgroups/new")]
        //[HttpPost]
        //public async Task<ActionResult<ProductGroupDTO>> CreateProductGroupAsync([FromBody] AddProductGroupCommand createProductGroupCommand)
        //{
        //    _logger.LogInformation(
        //        "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
        //        createProductGroupCommand.GetGenericTypeName(),
        //        nameof(createProductGroupCommand.ProductGroupName), createProductGroupCommand.ProductGroupId);

        //    return await _mediator.Send(createProductGroupCommand);
        //}

        //[Route("catalogitems/new")]
        //[HttpPost]
        //public async Task<ActionResult<CatalogItemDTO>> CreateCatalogItemAsync([FromBody] AddCatalogItemCommand createCatalogItemCommand)
        //{
        //    _logger.LogInformation(
        //        "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
        //        createCatalogItemCommand.GetGenericTypeName(),
        //        nameof(createCatalogItemCommand.CatalogId), createCatalogItemCommand.ProductId);

        //    return await _mediator.Send(createCatalogItemCommand);
        //}

        //[Route("marketplaces/new")]
        //[HttpPost]
        //public async Task<ActionResult<MarketPlaceDTO>> CreateMarketPlaceAsync([FromBody] AddMarketPlaceCommand createdMarketPlaceCommand)
        //{
        //    _logger.LogInformation(
        //        "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
        //        createdMarketPlaceCommand.GetGenericTypeName(),
        //        nameof(createdMarketPlaceCommand.PlanetId), createdMarketPlaceCommand.MarketPlaceId);

        //    return await _mediator.Send(createdMarketPlaceCommand);
        //}

        //[Route("planets/new")]
        //[HttpPost]
        //public async Task<ActionResult<PlanetDTO>> CreatePlanetAsync([FromBody] AddPlanetCommand createPlanetCommand)
        //{
        //    _logger.LogInformation(
        //        "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
        //        createPlanetCommand.GetGenericTypeName(),
        //        nameof(createPlanetCommand.PlanetId), createPlanetCommand.PlanetName);

        //    return await _mediator.Send(createPlanetCommand);
        //}

        //[HttpGet]
        //[Route("planets/{id:string}")]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType(typeof(Planet), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult<Planet>> ItemByIdAsync(string id)
        //{
        //    var planet = await _repo.GetItem(Guid.Parse(id));

        //    if (planet != null)
        //    {
        //        return planet;
        //    }

        //    return NotFound();
        //}

    }
}
