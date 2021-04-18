using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traveling.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TravelController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TravelController> _logger;

        public TravelController(IMediator mediator, ILogger<TravelController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET api/v1/[controller]/{posx}/{posy}/{posz}]
        [HttpGet]
        public async Task<IEnumerable<ObjectsInSectorModel>> Get(int posX, int posY, int posZ)
        {
            _logger.LogInformation($"Reading all objects in sector [{posX}, {posY}, {posZ}].");
            return await _mediator.Send(new ObjectsInSectorQuery());
        }
    }
}
