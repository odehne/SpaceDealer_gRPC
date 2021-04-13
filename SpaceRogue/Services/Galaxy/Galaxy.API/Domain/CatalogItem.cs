﻿using Cope.SpaceRogue.Galaxy.API.Domain;
using Cope.SpaceRogue.Galaxy.API.Domain.Events;
using Cope.SpaceRogue.Galaxy.API.Domain.SeedWork;
using Cope.SpaceRogue.InfraStructure;
using System;

namespace Cope.SpaceRogue.Galaxy.API.Domain
{
	public class CatalogItem : Entity
	{
		public Guid ID { get; set; }
		public string Title { get; set; }
		public decimal Price { get; set; }
		public virtual Guid ProductId { get; set; }
		public virtual Product Product { get; set; }
		public virtual MarketPlace Market { get; set; }


		public CatalogItem()
		{
		}

		public CatalogItem(Product product, string title, decimal price)
		{
			ID = Guid.NewGuid();
			Product = product;
			Title = title;
			Price = price;
			if (Price == default)
			{
				throw new InvalidEntityStateException(this, "The catalog item's price must have a value.");
			}
		}

		public void SetTitle(string title)
		{
			if (string.IsNullOrEmpty(title))
				throw new ArgumentException("Catalog item title must have a value.");
			Title = title;
			EnsureValidState();
			Raise(new CatalogItemTitleChangedEvent
			{
				Id = ID,
				Title = title
			}); 
		}

		public void SetPrice(decimal newPrice)
		{
			if (newPrice < 0)
				throw new ArgumentException("Catalog item's price must be greater than 0.");
			Price = newPrice;
		}

		protected override void EnsureValidState()
		{
			var valid = ID != default &&
						Product != null  && 
						!string.IsNullOrEmpty(Title) && 
						Price != default;
			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}