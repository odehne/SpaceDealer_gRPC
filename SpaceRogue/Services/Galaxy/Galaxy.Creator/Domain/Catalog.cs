using Cope.SpaceRogue.Galaxy.Creator.API.Domain.Events;
using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.Galaxy.Creator.Domain.SeedWork;
using Cope.SpaceRogue.InfraStructure;
using System;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Galaxy.Creator.API.Domain
{
	public class Catalog : Entity
	{
		public Guid ID { get; set; }
		public virtual ICollection<CatalogItem> CatalogItems { get; set; }

		public Catalog()
		{
			ID = Guid.NewGuid();
			CatalogItems = new List<CatalogItem>();
		}

		public void AddCatalogItem(Product product, string title, decimal price)
		{
			EnsureValidState();

			CatalogItems.Add(new CatalogItem(product, title, price));

			Raise(new CatalogItemAddedEvent
			{
				CatalogItemId = ID,
				ProductId = product.ID,
				Title = title,
				Price = price
			});
		}

		protected override void EnsureValidState()
		{
			var valid = ID == default &&
				CatalogItems != null;
			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}