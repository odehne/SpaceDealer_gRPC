using Cope.SpaceRogue.Galaxy.API.InfraStructure;
using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Galaxy.API.ViewModel;
using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Controllers
{

    [Route("api/v1/[controller]")]
	[ApiController]
	public class GalaxyController : ControllerBase
	{
		
		private readonly PlanetRepository _repo;

		public GalaxyController(PlanetRepository repo)
		{
			_repo = repo;
		}

        // GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("planets")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Planet>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Planet>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PlanetsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        {
            var lst = await _repo.GetItems();
            var totalItems = lst.Count;
                
            var itemsOnPage = lst
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            var model = new PaginatedItemsViewModel<Planet>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        // GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("planets/{x:int}/{y:int}/{z:int}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Planet>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Planet>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PlanetsAtPositionAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        {
            var lst = await _repo.GetItems();
            var totalItems = lst.Count;
                
            var itemsOnPage = lst
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            var model = new PaginatedItemsViewModel<Planet>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        [HttpGet]
        [Route("planets/{id:string}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Planet), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Planet>> ItemByIdAsync(string id)
        {
            var planet = await _repo.GetItem(Guid.Parse(id));
         
            if (planet != null)
            {
                return planet;
            }

            return NotFound();
        }

    }
}
