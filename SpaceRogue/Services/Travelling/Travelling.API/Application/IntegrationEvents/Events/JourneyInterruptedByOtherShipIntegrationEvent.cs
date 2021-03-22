using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public record JourneyInterruptedByOtherShipIntegrationEvent : IntegrationEvent
    {
        public string ShipId { get; set; }
        public string OtherShipId { get; set; }

        public JourneyInterruptedByOtherShipIntegrationEvent(string shipId, string otherShipId)
        {
            ShipId = shipId;
            OtherShipId = otherShipId;
        }

    }
}
