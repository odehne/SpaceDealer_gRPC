using System.Collections.Generic;

namespace Traveling.API.Controllers
{
	public class ObjectsInSectorModel
	{
		public IEnumerable<string> PlanetIds { get; set; }
		public IEnumerable<string> ShipIds { get; set; }

	}
}