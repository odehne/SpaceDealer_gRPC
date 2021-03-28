using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public class JourneyInterruptedByPiratesIntegrationEvent : IntegrationEvent
    {
        public string ShipId { get; set; }
        public string PirateShipId { get; set; }

        public JourneyInterruptedByPiratesIntegrationEvent(string shipId, string pirateShipId)
        {
            ShipId = shipId;
            PirateShipId = pirateShipId;
        }

    }
}
