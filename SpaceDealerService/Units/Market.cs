using System.Collections.Generic;

namespace SpaceDealer.Units
{
	public class Market : BaseUnit
	{
		public Planet Parent { get; set; }
		//public ProductsInStock ProductsToSell { get; set; }
		public ProductsInStock ProductsNeeded { get; set; }

		public Market(string name, List<KeyValuePair<string, string>> properties, Planet parent) : base(name, properties)
		{
			Parent = parent;
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
