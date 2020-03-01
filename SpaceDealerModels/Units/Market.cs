using System.Collections.Generic;

namespace SpaceDealerModels.Units
{
	public class Market : BaseUnit
	{
		public Planet Parent { get; set; }
		public ProductsInStock ProductsNeeded { get; set; }

		public Market(string name, Planet parent) : base(name)
		{
			Parent = parent;
			ProductsNeeded = new ProductsInStock();
		}

		public ProductsInStock GetProductsInStock()
		{
			var productsToSell = new ProductsInStock();
			foreach (var industry in Parent.Industries)
			{
				foreach(var p in industry.GeneratedProducts)
				{
					productsToSell.Add(p);
				}
			}
			return productsToSell;
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
