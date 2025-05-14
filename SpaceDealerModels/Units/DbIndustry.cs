using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SpaceDealerModels.Units
{

	public class DbIndustry : BaseUnit
	{

		[JsonProperty("generatedProducts")]
		public DbProductsInStock GeneratedProducts { get; set; }
		[JsonProperty("productsNeeded")]
		public DbProductsInStock ProductsNeeded { get; set; }

		public DbIndustry(string name) : base(name)
		{
			GeneratedProducts = new DbProductsInStock();
			ProductsNeeded = new DbProductsInStock();
		}

		public void AddGeneratedProduct(DbProductInStock product)
		{
			GeneratedProducts.Add(product);
		}

		public void AddNeededProduct(DbProductInStock product)
		{
			ProductsNeeded.Add(product);
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override void Update()
		{
			base.Update();
			foreach(var p in GeneratedProducts)
			{
				p.Amount += p.AmountGeneratedPerRound;
			}
		}

	}
}
