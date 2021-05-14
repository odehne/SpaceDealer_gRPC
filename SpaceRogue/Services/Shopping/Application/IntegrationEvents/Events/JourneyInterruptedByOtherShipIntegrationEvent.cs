using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Cope.SpaceRogue.Shopping.API.Application.IntegrationEvents.Events
{
	public class JourneyInterruptedByOtherShipIntegrationEvent : IntegrationEvent
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
