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
    public class PlayerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PlayerController> _logger;
        private readonly IPlayerRepository _repo;

        public PlayerController(IMediator mediator, ILogger<PlayerController> logger, IPlayerRepository repo)
        {
            _mediator = mediator;
            _logger = logger;
            _repo = repo;
        }

        // GET api/v1/[controller]/productgroups]
        [HttpGet]
        public async Task<IEnumerable<PlayerDTO>> Get()
        {
            return await _mediator.Send(new PlayersQuery());
        }
        // GET api/v1/[controller]/id]
        [HttpGet]
        [Route("{id}")]
        public async Task<PlayerDTO> GetPlayer(string id)
        {
            return await _mediator.Send(new PlayerQuery(id));
        }
    }
}
