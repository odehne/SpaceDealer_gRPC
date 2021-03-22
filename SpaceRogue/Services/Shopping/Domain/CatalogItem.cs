using Shopping.API.Domain.SeedWork;
using Shopping.API.InfraStructure;
using System;

namespace Cope.SpaceRogue.Traveling.API.Domain
{
	public class CatalogItem : Entity
	{
		public EntityId Id { get; }
		public EntityId CatalogId { get; }
		public EntityId ProductId { get; }
		public string Title { get; private set; }
		public decimal Price { get; private set; }

		public CatalogItem(EntityId id, EntityId catalogId, EntityId productId, string title, decimal price)
		{
			Id = id;
			CatalogId = catalogId;
			ProductId = productId;
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
		}

		public void SetPrice(decimal newPrice)
		{
			if (newPrice < 0)
				throw new ArgumentException("Catalog item's price must be greater than 0.");
			Price = newPrice;
		}

		protected override void EnsureValidState()
		{
			var valid = Id != default &&
						CatalogId != null &&
						ProductId != null && 
						!string.IsNullOrEmpty(Title) && 
						Price != default;
			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}