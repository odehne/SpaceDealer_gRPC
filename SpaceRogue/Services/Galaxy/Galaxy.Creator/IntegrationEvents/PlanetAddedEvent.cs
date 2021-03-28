using System;

namespace Cope.SpaceRogue.Galaxy.Creator.IntegrationEvents
{
	public class PlanetAddedEvent
	{
		public Guid PlanetId { get; set; }
		public string Name { get; set; }
		public int PosX { get; set; }
		public int PosY { get; set; }
		public int PosZ { get; set; }
	}
}
