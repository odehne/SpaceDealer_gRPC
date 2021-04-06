using System;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Cope.SpaceRogue.Galaxy.Creator.IntegrationEvents
{
	public class PlanetAddedEvent : IntegrationEvent
	{
		public string PlanetId { get; set; }
		public string Name { get; set; }
		public int PosX { get; set; }
		public int PosY { get; set; }
		public int PosZ { get; set; }
	}
}
