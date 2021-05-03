using Cope.SpaceRogue.InfraStructure;
using Cope.SpaceRogue.Travelling.API.Models;
using System.Collections.Generic;

namespace Traveling.API.Controllers
{
	public class ObjectsInSectorModel
	{
		public IEnumerable<PlanetModel> Planets { get; set; }
		public IEnumerable<ShipModel> Ships { get; set; }
		public Position Sector { get; set; }
	}
}