using Cope.SpaceRogue.InfraStructure;
using Cope.SpaceRogue.Maintenance.API.Domain.SeedWork;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Maintenance.API.Domain
{
	public class Fleet : Entity
	{

		public EntityId PlayerId { get; set; }
		public EntityId FleetId { get; set; }

		public List<EntityId> ShipIds { get; set; }

		public void AddShip(EntityId shipId)
		{

		}

		public void RemoveShip(EntityId shipId)
		{

		}

		protected override void EnsureValidState()
		{
			throw new System.NotImplementedException();
		}
	}
}