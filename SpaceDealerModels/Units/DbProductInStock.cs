using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpaceDealerModels.Units
{
	

	public class DbProductInStock : BaseUnit
	{
		[JsonProperty("amount")]
		public double Amount { get; set; }
		[JsonProperty("weight")]
		public double Weight { get; set; } // in tons
		[JsonProperty("amountGeneratedPerRound")]
		public double AmountGeneratedPerRound { get; set; } // Für gold sehr wenig, für Reis sehr viel
		[JsonProperty("pricePerTon")]
		public double PricePerTon { get; set; }

		public DbProductInStock()
		{
		}

		public DbProductInStock(string name) : base(name)
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

		public DbProductInStock(string name, double perRound, double totalAtStart, double weight, double suggestedRetailPrice) : base(name)
		{
			Weight = weight;
			Amount = totalAtStart;
			AmountGeneratedPerRound = perRound;
			PricePerTon = suggestedRetailPrice;
		}
	}
}
