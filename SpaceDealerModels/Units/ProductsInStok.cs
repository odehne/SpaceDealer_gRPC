using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
	public class ProductsInStock : List<ProductInStock>
	{
		public ProductInStock GetProductByName(string name)
		{
			return this.FirstOrDefault(x => x.Name.Equals(name));
		}

		public ProductsInStock()
		{

		}

		public override string ToString()
		{
			var ret = "";
			foreach (var product in this)
			{
				ret += $"{product.Name} ({product.Weight}t),";
			}
			ret = ret.TrimEnd(',');
			if (string.IsNullOrEmpty(ret))
				ret = "keine";
			return ret;
		}

		public ProductsInStock(ProductInStock p)
		{
			Add(p);
		}

		public ProductInStock AddProduct(ProductInStock p)
		{
			Add(p);
			return p;
		}

		public ProductInStock AddProduct(string name, double perRound, double totalAtStart, double weight, double suggestedRetailPrice)
		{
			var pis = new ProductInStock(name, perRound, totalAtStart, weight, suggestedRetailPrice);
			Add(pis);
			return pis;
		}

		public double GetTotalWeight()
		{
			var total = 0.0;
			foreach(var p in this)
			{
				total += p.GetTotalWeight();
			}
			return total;
		}
	}
}
