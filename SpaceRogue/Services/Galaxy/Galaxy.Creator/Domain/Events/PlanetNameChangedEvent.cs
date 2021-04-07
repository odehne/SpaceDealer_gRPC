using System;

namespace Cope.SpaceRogue.Galaxy.Creator.Domain.Events
{
	public class PlanetNameChangedEvent
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}
