using System;
using System.Collections.Generic;

namespace SpaceDealerModels.Units
{

	public class Industry : BaseUnit
	{
		public ProductsInStock GeneratedProducts { get; set; }
		public ProductsInStock ProductsNeeded { get; set; }

		public Industry(string name, List<KeyValuePair<string, string>> properties) : base(name, properties)
		{
			GeneratedProducts = new ProductsInStock();
			ProductsNeeded = new ProductsInStock();
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
