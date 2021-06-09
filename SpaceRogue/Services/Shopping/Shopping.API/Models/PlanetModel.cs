
using Cope.SpaceRogue.Infrastructure;
using System;

namespace Cope.SpaceRogue.Shopping.API.Models
{
	public class PlanetModel
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public Position Sector { get; set; }
		public MarketPlaceModel Market { get; set; }

		public PlanetModel()
		{
			Market = new MarketPlaceModel();
			Sector = new Position(0, 0, 0);
		}
	}

}