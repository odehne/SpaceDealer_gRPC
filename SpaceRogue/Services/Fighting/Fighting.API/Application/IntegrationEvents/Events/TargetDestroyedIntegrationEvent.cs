using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public class TargetDestroyedIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }

		public TargetDestroyedIntegrationEvent(string shipId) => ShipId = shipId;
	}

}
