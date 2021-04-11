using System;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Cope.SpaceRogue.Galaxy.API.IntegrationEvents
{
	public class MarketPlaceAdded : IntegrationEvent
	{
		public string MarketPlaceId { get; set; }
		public string PlanetId { get; set; }
	}
}
