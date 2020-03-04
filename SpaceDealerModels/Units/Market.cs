using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpaceDealerModels.Units
{
	public class Market : BaseUnit
	{
		[JsonIgnore]
		public Planet Parent { get; set; }

		public Market()
		{

		}
	
		public Market(string name, Planet parent) : base(name)
		{
			Parent = parent;
		}
		public ProductsInStock GetProductsToSell()
		{
			var productsToSell = new ProductsInStock();
			foreach (var industry in Parent.Industries)
			{
				foreach (var p in industry.ProductsNeeded)
				{
					productsToSell.Add(p);
				}
			}
			return productsToSell;
		}

		public ProductsInStock GetProductsToBuy()
		{
			var productsToBuy = new ProductsInStock();
			foreach (var industry in Parent.Industries)
			{
				foreach(var p in industry.GeneratedProducts)
				{
					productsToBuy.Add(p);
				}
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
