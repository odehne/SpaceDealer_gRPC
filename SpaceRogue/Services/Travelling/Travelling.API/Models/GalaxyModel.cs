using Cope.SpaceRogue.Traveling.API.Models;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Traveling.API.Models
{
	public class GalaxyModel 
	{ 
		public List<PlanetModel> Planets { get; set; }
		public List<ShipModel> OtherShips { get; set; }

	}
}