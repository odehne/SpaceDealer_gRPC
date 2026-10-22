using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace SpaceDealerModels.Units
{
	

	public class DbProductInStock : BaseUnit
	{
		[JsonPropertyName("amount")]
		public double Amount { get; set; }
		[JsonPropertyName("weight")]
		public double Weight { get; set; } // in tons
		[JsonPropertyName("amountGeneratedPerRound")]
		public double AmountGeneratedPerRound { get; set; } // Für gold sehr wenig, für Reis sehr viel
		[JsonPropertyName("pricePerTon")]
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
