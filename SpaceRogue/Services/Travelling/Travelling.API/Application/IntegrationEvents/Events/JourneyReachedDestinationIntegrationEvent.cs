using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public record JourneyReachedDestinationIntegrationEvent : IntegrationEvent
    {
        public string ShipId { get; set; }
        public string PlanetId { get; set; }

        public JourneyReachedDestinationIntegrationEvent(string shipId, string planetId)
        {
            ShipId = shipId;
            PlanetId = planetId;
        }
            
    }
}
