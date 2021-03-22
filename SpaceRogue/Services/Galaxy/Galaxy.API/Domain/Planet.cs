using Cope.SpaceRogue.Galaxy.API.InfraStructure;
using System;

namespace Cope.SpaceRogue.Galaxy.API.Model
{
	public class Planet : Entity
	{
		public EntityId PlanetId { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public EntityId MarketPlaceId { get; set; }

		public Position AstronomicalPosition {get; set;}

		protected override void EnsureValidState()
		{
			throw new NotImplementedException();
		}
	}
}
