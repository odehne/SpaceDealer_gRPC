using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public class ShipTookAHitIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }

		public ShipTookAHitIntegrationEvent(string shipId) => ShipId = shipId;
	}

}
