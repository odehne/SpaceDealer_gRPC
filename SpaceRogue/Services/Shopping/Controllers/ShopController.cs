using Cope.SpaceRogue.Shopping.API.Models;
using Cope.SpaceRogue.Shopping.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Shopping.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ShopController> _logger;

        public ShopController(IMediator mediator, ILogger<ShopController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET api/v1/[controller]/{markedPlaceId}]
        [HttpGet]
        public async Task<MarketPlaceModel> Get(string markedPlaceId)
        {
            _logger.LogInformation($"Reading market [{markedPlaceId}].");
            return await _mediator.Send(new MarketPlaceQuery(markedPlaceId));
        }
    }
}
