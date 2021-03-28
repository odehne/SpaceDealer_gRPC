using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public class ShieldsCriticalIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }

		public ShieldsCriticalIntegrationEvent(string shipId) => ShipId = shipId;
	}

}
