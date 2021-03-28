using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public class HullCriticalIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }

		public HullCriticalIntegrationEvent(string shipId) => ShipId = shipId;
	}

}
