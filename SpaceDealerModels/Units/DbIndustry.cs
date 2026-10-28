using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace SpaceDealerModels.Units
{

	public class DbIndustry : BaseUnit
	{

		[JsonPropertyName("generatedProducts")]
		public DbProductsInStock GeneratedProducts { get; set; }
		[JsonPropertyName("productsNeeded")]
		public DbProductsInStock ProductsNeeded { get; set; }

        public DbIndustry() : base()
		{
			GeneratedProducts = new DbProductsInStock();
			ProductsNeeded = new DbProductsInStock();
		}

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

		public bool HasEnoughProducts(DbProductsInStock products)
        {
            foreach (var product in products)
            {
                var neededProduct = ProductsNeeded.GetById(product.Id);
                if (neededProduct == null || product.Amount < neededProduct.Amount)
                {
                    return false;
                }
            }
            return true;
        }

        public void DeductItem(string productId, double quantity)
        {
            var product = GeneratedProducts.GetById(productId);
            if (product != null)
            {
                product.Amount -= quantity;
                if (product.Amount < 0)
                {
				    product.Amount = 0;
				}
            }
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
