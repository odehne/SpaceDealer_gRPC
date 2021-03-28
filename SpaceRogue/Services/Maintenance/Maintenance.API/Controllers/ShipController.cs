using Cope.SpaceRogue.Maintenance.API.Domain;
using Cope.SpaceRogue.Maintenance.API.Infrastructure;
using Cope.SpaceRogue.Maintenance.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Maintenance.API.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class ShipController : ControllerBase
	{
        private readonly ShipRepository _shipContext;

        public ShipController(ShipRepository shipContext)
        {
            _shipContext = shipContext;
        }

        [HttpGet]
        [Route("items/{id:string}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Ship), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Ship>> ItemByIdAsync(string id)
        {
            var ship = await _shipContext.GetItem(id);

            if (ship != null)
            {
                return ship;
            }

            return NotFound();
        }
    }
}
