using Cope.SpaceRogue.Fighting.API.Models;
using Cope.SpaceRogue.Fighting.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FightController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FightController> _logger;

        public FightController(IMediator mediator, ILogger<FightController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET api/v1/[controller]/]
        [HttpGet]
        public async Task<List<FightModel>> Get()
        {
            _logger.LogInformation($"Reading all fights in sector.");
            return await _mediator.Send(new FightsQuery());
        }
    }
}
