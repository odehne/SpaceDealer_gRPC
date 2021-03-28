using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.Galaxy.Creator.Domain.SeedWork;
using Cope.SpaceRogue.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

		public MarketPlace(Guid marketPlaceId, List<CatalogItem> offerings, List<CatalogItem> demands)
		{
			ID = marketPlaceId;
			ProductOfferings = offerings;
			ProductDemands = demands;

			EnsureValidState();

			Raise(new Events.MarketPlaceAddedEvent
			{
				MarketPlaceId = marketPlaceId
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