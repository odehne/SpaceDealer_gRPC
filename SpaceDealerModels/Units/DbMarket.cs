using System.Text.Json.Serialization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SpaceDealerModels.Units
{
	//public class DbMarket : BaseUnit
	//{
	//	public DbMarket()
	//	{

	//	}
	
	//	public DbMarket(string name) : base(name)
	//	{
	//		//Parent = parent;
	//	}

	//	public DbProductsInStock GetProductsToSell()
	//	{
	//		var productsToSell = new DbProductsInStock();
	//		foreach (var p in Parent.Industry.ProductsNeeded)
	//		{
	//			productsToSell.Add(p);
	//		}
	//		return productsToSell;
	//	}

	//	public DbProductsInStock GetProductsToBuy()
	//	{
	//		var productsToBuy = new DbProductsInStock();
	//		foreach (var p in Parent.Industry.GeneratedProducts)
	//		{
	//			productsToBuy.Add(p);
	//		}
	//		return productsToBuy;
	//	}

	//	public override int GetHashCode()
	//	{
	//		return base.GetHashCode();
	//	}

	//	public override string ToString()
	//	{
	//		return base.ToString();
	//	}

	//	public override void Update()
	//	{
	//		base.Update();
	//	}
	//}
}
