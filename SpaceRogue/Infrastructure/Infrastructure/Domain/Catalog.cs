using Cope.SpaceRogue.Infrastructure.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cope.SpaceRogue.Infrastructure.Domain
{
	public class Catalog : Entity
	{
		public Guid ID { get; set; }
		public virtual ICollection<CatalogItem> CatalogItems { get; set; }


		public Catalog(Guid id)
		{
			ID = id;
			CatalogItems = new List<CatalogItem>();
		}

		public Catalog()
		{
			ID = Guid.NewGuid();
			CatalogItems = new List<CatalogItem>();
		}

		public void AddCatalogItem(Product product, string title, int percentValue = 0)
		{
			EnsureValidState();

			var price = CalculatePrice(product, percentValue);

			CatalogItems.Add(new CatalogItem(product, title, (decimal)price, ID));

			//Raise(new CatalogItemAddedEvent
			//{
			//	CatalogItemId = ID,
			//	ProductId = product.ID,
			//	Title = title,
			//	Price = (decimal)price
			//});
		}

        private double CalculatePrice(Product product, int percentValue)
        {
			var newPrice = 0.0;
            var currentPrice = product.PricePerUnit;
			if(percentValue==100)
				return currentPrice;

			if(percentValue>0){
				newPrice = ((currentPrice * percentValue) / 100) + currentPrice;
			}else{
				newPrice = currentPrice - ((currentPrice * (percentValue * -1))) / 100;
			}
			return newPrice;
        }


        protected override void EnsureValidState()
		{
			var valid = ID != default &&
				CatalogItems != null;
			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
			var s = CatalogItems.Select(x=>x.Title).ToArray();
			return string.Join(", ", s);
        }
    }
}