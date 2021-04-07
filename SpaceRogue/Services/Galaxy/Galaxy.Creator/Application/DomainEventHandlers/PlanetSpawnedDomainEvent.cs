using MediatR;

namespace Cope.SpaceRogue.Galaxy.Application.DomainEventHandlers
{
	/// <summary>
	/// Event used when a new planet was spawned
	/// </summary>
	public class PlanetSpawnedDomainEvent : INotification
	{
		public string ID { get; }
		public string Name { get; }
		public int PosX { get; }
		public int PosY { get; }
		public int PosZ { get; }

		public PlanetSpawnedDomainEvent(string iD, string name, int posX, int posY, int posZ)
		{
			ID = iD;
			Name = name;
			PosX = posX;
			PosY = posY;
			PosZ = posZ;
		}
	}
}