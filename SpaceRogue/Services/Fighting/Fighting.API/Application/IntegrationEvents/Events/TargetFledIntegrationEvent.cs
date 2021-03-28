using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public class TargetFledIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }

		public TargetFledIntegrationEvent(string shipId) => ShipId = shipId;
	}

}
