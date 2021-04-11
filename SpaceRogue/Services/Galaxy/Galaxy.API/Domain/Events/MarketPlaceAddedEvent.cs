using System;

namespace Cope.SpaceRogue.Galaxy.API.Domain.Events
{
	public class MarketPlaceAddedEvent
	{
		public Guid PlanetId { get; set; }
		public Guid MarketPlaceId { get; set; }
	}

}
