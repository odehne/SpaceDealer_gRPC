using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Cope.SpaceRogue.Galaxy.Application.DomainEventHandlers
{
	internal class PlanetSpawnedIntegrationEvent : IntegrationEvent
	{
		public string ID { get;  }
		public string Name { get; }
		public int PosX { get; }
		public int PosY { get; }
		public int PosZ { get; }

		public PlanetSpawnedIntegrationEvent(string id, string name,  int posX, int posY, int posZ)
		{
			ID = id;
			Name = name;
			PosX = posX;
			PosY = posY;
			PosZ = posZ;
		}
	}
}