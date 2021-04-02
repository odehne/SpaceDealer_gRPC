using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.Galaxy.Creator.Domain.SeedWork;
using Cope.SpaceRogue.InfraStructure;
using System;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Galaxy.Creator.API.Domain
{

	public class MarketPlace : Entity
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public virtual List<CatalogItem> ProductOfferings { get; set; }
		public virtual List<CatalogItem> ProductDemands { get; set; }
		public virtual Planet Planet { get; set; }

		public MarketPlace()
		{
			ID = Guid.NewGuid();
		}

		public MarketPlace(List<CatalogItem> offerings, List<CatalogItem> demands)
		{
			ID = Guid.NewGuid();
			ProductOfferings = offerings;
			ProductDemands = demands;
			
			EnsureValidState();

			Raise(new Events.MarketPlaceAddedEvent
			{
				MarketPlaceId = ID
			});
		}

		protected override void EnsureValidState()
		{
			var valid = ID != default &&
				ProductDemands != default  &&
				ProductOfferings != default;
			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}

	}
}