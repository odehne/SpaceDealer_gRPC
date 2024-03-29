﻿using Newtonsoft.Json;

namespace SpaceDealerModels.Units
{
	public class DbMarket : BaseUnit
	{
		[JsonIgnore]
		public DbPlanet Parent { get; set; }

		public DbMarket()
		{

		}
	
		public DbMarket(string name, DbPlanet parent) : base(name)
		{
			Parent = parent;
		}
		public DbProductsInStock GetProductsToSell()
		{
			var productsToSell = new DbProductsInStock();
			foreach (var p in Parent.Industry.ProductsNeeded)
			{
				productsToSell.Add(p);
			}
			return productsToSell;
		}

		public DbProductsInStock GetProductsToBuy()
		{
			var productsToBuy = new DbProductsInStock();
			foreach (var p in Parent.Industry.GeneratedProducts)
			{
				productsToBuy.Add(p);
			}
			return productsToBuy;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override void Update()
		{
			base.Update();
		}
	}
}
