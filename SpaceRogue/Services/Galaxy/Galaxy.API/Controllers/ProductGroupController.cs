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
    public class ProductGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductGroupsController> _logger;
        private readonly IProductGroupRepository _repo;

        public ProductGroupsController(IMediator mediator, ILogger<ProductGroupsController> logger, IProductGroupRepository repo)
        {
            _mediator = mediator;
            _logger = logger;
            _repo = repo;
        }

        // GET api/v1/[controller]/productgroups]
        [HttpGet]
        public async Task<IEnumerable<ProductGroupDTO>> Get()
        {
            return await _mediator.Send(new ProductGroupsQuery());
        }
        // GET api/v1/[controller]/id]
        [HttpGet]
        [Route("{id}")]
        public async Task<ProductGroupDTO> GetProductGroup(string id)
        {
            return await _mediator.Send(new ProductGroupQuery(id));
        }

        // GET api/v1/[controller]/productgroups]
        [HttpGet]
        [Route("{id}/products")]
        public async Task<IEnumerable<ProductDTO>> GetProducts(string id)
        {
            return await _mediator.Send(new ProductsInGroupQuery(id));
        }

        [Route("new")]
        [HttpPost]
        public async Task<ActionResult<ProductGroupDTO>> CreateProductAsync([FromBody] AddProductGroupCommand addProductGroupCommand)
        {
           return await _mediator.Send(addProductGroupCommand);
        }
    }
}
