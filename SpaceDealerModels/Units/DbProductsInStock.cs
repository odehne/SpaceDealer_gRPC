using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{

	public class ProductCategory
	{
		public string Name { get; set; }
		public List<DbProductsInStock> Needed { get; set; }
		
    }

    public class DbProductsInStock : List<DbProductInStock>
	{
		public DbProductInStock GetByName(string name)
		{

            foreach (var p in this)
            {
                if (!string.IsNullOrEmpty(p.Name) && p.Name.Equals(name))
                    return p;
            }

			return null;
		}

        public DbProductInStock GetById(string id)
        {

            foreach (var p in this)
            {
                if (!string.IsNullOrEmpty(p.Id) && p.Id.Equals(id))
                    return p;
            }

            return null;
        }

        public DbProductsInStock()
		{

		}

		public DbProductInStock GetRandomProduct()
		{
			if(this.Count == 0) return null;

            var random = new Random();
			int index = random.Next(this.Count);

			return this[index];
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
            var pis = new DbProductInStock(name, perRound, totalAtStart, weight, suggestedRetailPrice)
            {
                PicturePath = ""
            };
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
