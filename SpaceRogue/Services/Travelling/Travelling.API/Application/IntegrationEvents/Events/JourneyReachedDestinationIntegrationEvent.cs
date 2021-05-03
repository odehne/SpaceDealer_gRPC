using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Cope.SpaceRogue.Travelling.API.Application.IntegrationEvents.Events
{
	public class JourneyReachedDestinationIntegrationEvent : IntegrationEvent
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
