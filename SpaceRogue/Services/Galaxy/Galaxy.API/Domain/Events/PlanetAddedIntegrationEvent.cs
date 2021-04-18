using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System;

namespace Cope.SpaceRogue.Galaxy.API.Domain.Events
{

	public class PlanetAddedIntegrationEvent : IntegrationEvent
	{ 
		public string PlanetId { get; }

		public PlanetAddedIntegrationEvent(string planetId)
		{
			PlanetId = planetId;
		}
	}
}
