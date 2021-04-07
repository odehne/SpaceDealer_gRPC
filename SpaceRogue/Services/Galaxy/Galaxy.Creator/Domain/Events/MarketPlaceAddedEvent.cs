using System;

namespace Cope.SpaceRogue.Galaxy.Creator.Domain.Events
{
	public class MarketPlaceAddedEvent
	{
		public Guid PlanetId { get; set; }
		public Guid MarketPlaceId { get; set; }
	}

}
