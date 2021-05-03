using Cope.SpaceRogue.Travelling.API.Models;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Travelling.API.Models
{
	public class GalaxyModel 
	{ 
		public List<PlanetModel> Planets { get; set; }
		public List<ShipModel> Ships { get; set; }
	}
}