using System;

namespace Cope.SpaceRogue.Galaxy.API.Domain.Events
{
	public class PlanetNameChangedEvent
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}
