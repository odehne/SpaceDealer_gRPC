using System;

namespace Shopping.API.Domain.Events
{
	public class MarketOpened
	{
		public Guid Id { get; set; }
		public Guid PlanetId { get; set; }

		public Guid ProductOfferingsCatalog { get; set; }
		public Guid ProductDemandsCatalog { get; set; }

	}

	public class MarketClosed
	{
		public Guid Id { get; set; }
		public Guid PlanetId { get; set; }
	}
}
