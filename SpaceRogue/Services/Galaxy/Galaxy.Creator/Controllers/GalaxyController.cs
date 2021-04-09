using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Galaxy.API.ViewModel;
using Cope.SpaceRogue.Galaxy.Creator.Application.Commands;
using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using Galaxy.Creator.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Controllers
{
	//// GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]

	[Route("api/v1/[controller]")]
	[ApiController]
	public class GalaxyController : ControllerBase
	{
        private readonly IMediator _mediator;
        private readonly ILogger<GalaxyController> _logger;
        private readonly IPlanetRepository _repo;

		public GalaxyController(IMediator mediator, ILogger<GalaxyController> logger, IPlanetRepository repo)
		{
			_mediator = mediator;
			_logger = logger;
			_repo = repo;
		}

        // GET api/v1/[controller]/products]
        [HttpGet]
        [Route("products")]
        public async Task<IEnumerable<ProductDTO>> Get()
        {
            return await _mediator.Send(new ProductsQuery());
        }

        //// GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
        //[HttpGet]
        //[Route("planets")]
        //[ProducesResponseType(typeof(PaginatedItemsViewModel<Planet>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(IEnumerable<Planet>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //public async Task<IActionResult> PlanetsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        //{
        //    var lst = await _repo.GetItems();
        //    var totalItems = lst.Count;

        //    var itemsOnPage = lst
        //        .OrderBy(c => c.Name)
        //        .Skip(pageSize * pageIndex)
        //        .Take(pageSize);

        //    var model = new PaginatedItemsViewModel<Planet>(pageIndex, pageSize, totalItems, itemsOnPage);

        //    return Ok(model);
        //}

        //// GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
        //[HttpGet]
        //[Route("planets/{x:int}/{y:int}/{z:int}")]
        //[ProducesResponseType(typeof(PaginatedItemsViewModel<Planet>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(IEnumerable<Planet>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //public async Task<IActionResult> PlanetsAtPositionAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        //{
        //    var lst = await _repo.GetItems();
        //    var totalItems = lst.Count;

        //    var itemsOnPage = lst
        //        .OrderBy(c => c.Name)
        //        .Skip(pageSize * pageIndex)
        //        .Take(pageSize);

        //    var model = new PaginatedItemsViewModel<Planet>(pageIndex, pageSize, totalItems, itemsOnPage);

        //    return Ok(model);
        //}

        //[Route("products/new")]
        //[HttpPost]
        //public async Task<ActionResult<ProductDTO>> CreateProductAsync([FromBody] AddProductCommand createProductCommand)
        //{
        //    _logger.LogInformation(
        //        "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
        //        createProductCommand.GetGenericTypeName(),
        //        nameof(createProductCommand.ProductName), createProductCommand.ProductGroupId, createProductCommand.PricePerUnit);

        //    return await _mediator.Send(createProductCommand);
        //}

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
