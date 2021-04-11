using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Galaxy.API.Domain;
using Cope.SpaceRogue.Galaxy.API.Domain.SeedWork;
using Cope.SpaceRogue.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cope.SpaceRogue.Galaxy.API.Domain
{

	public class MarketPlace : Entity
	{
		[Key]
		public Guid ID { get; set; }
		public string Name { get; set; }

		public virtual Catalog ProductOfferings { get; set; }
		public virtual Catalog ProductDemands { get; set; }

		public MarketPlace()
		{
			ID = Guid.NewGuid();
		}

		public MarketPlace(string name, Catalog offerings, Catalog demands)
		{
			ID = Guid.NewGuid();
			Name = name;
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
				!string.IsNullOrEmpty(Name)  &&
				ProductDemands != null  &&
				ProductOfferings != null;
			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}

	}
}