using Cope.SpaceRogue.Galaxy.API.InfraStructure;
using Cope.SpaceRogue.InfraStructure;
using System;

namespace Cope.SpaceRogue.Galaxy.API.Model
{
	public class Planet : Entity
	{
		public Guid PlanetId { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public Guid MarketPlaceId { get; set; }

		public int PosX { get; set; }
		public int PosY { get; set; }
		public int PosZ { get; set; }

		protected override void EnsureValidState()
		{
			throw new NotImplementedException();
		}
	}
}
