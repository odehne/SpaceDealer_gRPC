using Cope.SpaceRogue.Galaxy.Creator.Application.Commands;
using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using Galaxy.Creator.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _repo;

        public ProductController(IMediator mediator, ILogger<ProductController> logger, IProductRepository repo)
        {
            _mediator = mediator;
            _logger = logger;
            _repo = repo;
        }

        // GET api/v1/[controller]/productgroups]
        [HttpGet]
        public async Task<IEnumerable<ProductDTO>> Get()
        {
            return await _mediator.Send(new ProductsQuery());
        }
        // GET api/v1/[controller]/id]
        [HttpGet]
        [Route("{id}")]
        public async Task<ProductDTO> GetProduct(string id)
        {
            return await _mediator.Send(new ProductQuery(id));
        }
    }
}
