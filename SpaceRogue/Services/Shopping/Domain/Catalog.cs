using Shopping.API.Domain.Events;
using Shopping.API.Domain.SeedWork;
using Shopping.API.InfraStructure;
using System;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Traveling.API.Domain
{
	public class Catalog : Entity
	{
		public EntityId Id { get; private set; }
		public EntityId OwnerId { get; private set; }
		public List<CatalogItem> Items { get; private set; }

		public Catalog(EntityId ownerId, EntityId id)
		{
			Id = id;
			OwnerId = ownerId;
			Items = new List<CatalogItem>();
		}

		public void AddCatalogItem(EntityId productId, string title, decimal price)
		{
			EnsureValidState();

			Items.Add(new CatalogItem(new EntityId(Guid.NewGuid()), Id, productId, title, price));

			Raise(new Events.CatalogItemAdded
			{
				Id = Id.Value,
				ProductId = productId.Value,
				Price = price
			});
		}

		protected override void EnsureValidState()
		{
			var valid = Id == default &&
				OwnerId != null &&
				Items != null;
			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}