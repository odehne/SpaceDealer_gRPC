using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
	public class DbProductsInStock : List<DbProductInStock>
	{
		public DbProductInStock GetProductByName(string name)
		{

            foreach (var p in this)
            {
                if (!string.IsNullOrEmpty(p.Name) && p.Name.Equals(name))
                    return p;
            }

			return null;
		}

		public DbProductsInStock()
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

		public DbProductsInStock(DbProductInStock p)
		{
			Add(p);
		}

		public DbProductInStock AddProduct(DbProductInStock p)
		{
			Add(p);
			return p;
		}

		public DbProductInStock AddProduct(string name, double perRound, double totalAtStart, double weight, double suggestedRetailPrice)
		{
			var pis = new DbProductInStock(name, perRound, totalAtStart, weight, suggestedRetailPrice);
			pis.PicturePath = "";
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
