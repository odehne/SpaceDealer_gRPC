using System;

namespace Cope.SpaceRogue.Galaxy.Creator.IntegrationEvents
{
	public class MarketPlaceAdded
	{
		public Guid MarketPlaceId { get; set; }
		public Guid PlanetId { get; set; }
	}
}
