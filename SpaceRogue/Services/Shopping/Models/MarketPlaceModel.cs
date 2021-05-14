using Cope.SpaceRogue.Infrastructure;
using System;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Shopping.API.Models
{
	public class MarketPlaceModel
	{
		public Guid ID { get; set; }
		public Guid PlanetId { get; set; }
		public string Name { get; set; }
		public string PlanetName { get; set; }
		public Position Sector { get; set; } 
		public CatalogModel ProductOfferings { get; set; }
		public CatalogModel ProductDemands { get; set; }

		public MarketPlaceModel()
		{
			ProductDemands = new CatalogModel();
			ProductOfferings = new CatalogModel();
		}
	}

}