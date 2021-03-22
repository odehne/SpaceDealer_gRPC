using Cope.SpaceRogue.Galaxy.API.InfraStructure;
using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Galaxy.API.ViewModel;
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
		
		private readonly GalaxyRepository _galaxyContext;

		public GalaxyController(GalaxyRepository galaxyContext)
		{
			_galaxyContext = galaxyContext;
		}

        // GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Planet>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Planet>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PlanetsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        {
            var lst = await _galaxyContext.Read();
            var totalItems = lst.Count;

                
            var itemsOnPage = lst
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            var model = new PaginatedItemsViewModel<Planet>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        [HttpGet]
        [Route("items/{id:string}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Planet), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Planet>> ItemByIdAsync(string id)
        {
            var planet = await _galaxyContext.GetItem(id);
         
            if (planet != null)
            {
                return planet;
            }

            return NotFound();
        }

        // GET api/v1/[controller]/items/withname/samplename[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/withname/{name:minlength(1)}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Planet>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Planet>>> ItemsWithNameAsync(string name, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var lst = await _galaxyContext.GetItemsByName(name);
            var totalItems = lst.Count;

            var itemsOnPage = await _galaxyContext.Read();

            itemsOnPage.Where(c => c.Name.StartsWith(name))
                 .Skip(pageSize * pageIndex)
                 .Take(pageSize);

            return new PaginatedItemsViewModel<Planet>(pageIndex, pageSize, totalItems, lst);
        }
    }
}
