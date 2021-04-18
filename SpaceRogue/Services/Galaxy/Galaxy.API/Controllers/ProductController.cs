using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using Galaxy.Creator.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController> _logger;
    
        public ProductController(IMediator mediator, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET api/v1/[controller]/productgroups]
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> Get()
        {
            return await _mediator.Send(new ProductsQuery());
        }

        // GET api/v1/[controller]/id]
        [HttpGet]
        [Route("{id}")]
        public async Task<ProductDto> GetProduct(string id)
        {
            return await _mediator.Send(new ProductQuery(id));
        }

		[Route("new")]
		[HttpPost]
		public async Task<ActionResult<ProductDto>> CreateProductAsync([FromBody] AddProductCommand addProductCommand)
		{
			_logger.LogInformation(
				"----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
				addProductCommand.GetGenericTypeName(),
				nameof(addProductCommand.ProductName), addProductCommand.ProductGroupId, addProductCommand.PricePerUnit);

			return await _mediator.Send(addProductCommand);
		}
	}
}
