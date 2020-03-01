using System.Collections.Generic;

namespace SpaceDealerModels.Units
{
	public class ProductInStock : BaseUnit
	{
		public double Amount { get; set; }
		public double Weight { get; set; } // in tons
		public double AmountGeneratedPerRound { get; set; } // Für gold sehr wenig, für Reis sehr viel
		public double PricePerTon { get; set; }


		public ProductInStock(string name) : base(name)
		{
			
		}

		public double GetTotalWeight() 
		{
			return Amount * Weight;
		}

		public double GetTotalPrice()
		{
			return Amount * Weight * PricePerTon;
		}

		public ProductInStock(string name, double perRound, double totalAtStart, double weight, double suggestedRetailPrice) : base(name)
		{
			Weight = weight;
			Amount = totalAtStart;
			AmountGeneratedPerRound = perRound;
			PricePerTon = suggestedRetailPrice;
		}
	}
}
