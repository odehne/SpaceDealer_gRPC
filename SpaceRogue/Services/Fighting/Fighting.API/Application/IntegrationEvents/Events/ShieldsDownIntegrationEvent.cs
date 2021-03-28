using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public class ShieldsDownIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }

		public ShieldsDownIntegrationEvent(string shipId) => ShipId = shipId;
	}

}
