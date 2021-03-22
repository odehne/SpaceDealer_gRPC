using Shopping.API.Domain.Events;
using Shopping.API.Domain.SeedWork;
using Shopping.API.InfraStructure;

namespace Cope.SpaceRogue.Traveling.API.Domain
{
	public class MarketPlace : Entity
	{
		public EntityId Id { get; private set; }
		public string Name { get; set; }

		public EntityId PlanetId { get; private set; }
		public Catalog ProductOfferings { get; private set; }
		public Catalog ProductDemands { get; private set; }

		public MarketPlace(EntityId id, EntityId planetId, Catalog offerings, Catalog demands)
		{
			Id = id;
			PlanetId = planetId;
			ProductOfferings = offerings;
			ProductDemands = demands;

			EnsureValidState();

			Raise(new MarketOpened
			{
				Id = id.Value,
				PlanetId = planetId.Value,
				ProductDemandsCatalog = ProductDemands.Id.Value,
				ProductOfferingsCatalog = ProductOfferings.Id.Value
			});
		}

		protected override void EnsureValidState()
		{
			var valid = Id != null &&
				ProductDemands != null &&
				ProductOfferings != null;
			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}

	}
}