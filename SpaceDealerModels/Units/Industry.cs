using System;
using System.Collections.Generic;

namespace SpaceDealerModels.Units
{

	public class Industry : BaseUnit
	{
		public ProductsInStock GeneratedProducts { get; set; }
		public ProductsInStock ProductsNeeded { get; set; }

		public Industry(string name) : base(name)
		{
			GeneratedProducts = new ProductsInStock();
			ProductsNeeded = new ProductsInStock();
		}

		public void AddGeneratedProduct(ProductInStock product)
		{
			GeneratedProducts.Add(product);
		}

		public void AddNeededProduct(ProductInStock product)
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
