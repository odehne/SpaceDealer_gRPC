﻿using Cope.SpaceRogue.Galaxy.Creator.API.Domain.Events;
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

		public void AddCatalogItem(Product product, string title, int percentValue = 0)
		{
			EnsureValidState();

			var price = CalculatePrice(product, percentValue);

			CatalogItems.Add(new CatalogItem(product, title, (decimal)price));

			Raise(new CatalogItemAddedEvent
			{
				CatalogItemId = ID,
				ProductId = product.ID,
				Title = title,
				Price = (decimal)price
			});
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

        private double AddPercentOnProductPrice(Product product, int plusPercentOnPrice)
        {
            throw new NotImplementedException();
        }

        protected override void EnsureValidState()
		{
			var valid = ID != default &&
				CatalogItems != null;
			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}