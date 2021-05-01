using Cope.SpaceRogue.Galaxy.Creator.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Creator.App.Model
{
	public class PlanetModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public Position Sector { get; set; }
		public string MarketId { get; set; }
	}
}
