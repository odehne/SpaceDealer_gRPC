using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using Galaxy.Creator.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Controllers
{
    [Route("api/v1/[controller]")]
	[ApiController]
	public class MarketPlaceController : ControllerBase
	{
        private readonly IMediator _mediator;
        private readonly ILogger<MarketPlaceController> _logger;
        private readonly IMarketPlaceRepository _repo;

        public MarketPlaceController(IMediator mediator, ILogger<MarketPlaceController> logger, IMarketPlaceRepository repo)
        {
            _mediator = mediator;
            _logger = logger;
            _repo = repo;
        }

        // GET api/v1/[controller]/id/marketplace]
        [HttpGet]
        [Route("{id}")]
        public async Task<MarketPlaceDto> Get(string id)
        {
            return await _mediator.Send(new MarketPlaceQuery(id));
        }

    }
}
