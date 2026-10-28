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
		/// <summary>Planetenspezifische Marge in Prozent, die bereits in <see cref="PricePerTon"/> enthalten ist.</summary>
		[JsonPropertyName("interest")]
		public double Interest { get; set; }

		/// <summary>Preis ohne planetenspezifische Marge (so wie in der Products-Tabelle gespeichert).</summary>
		[JsonIgnore]
		public double BasePricePerTon => PricePerTon / (1 + Interest / 100);

		/// <summary>Setzt die Marge und rechnet sie auf den Basispreis auf.</summary>
		public void ApplyInterest(double interest)
		{
			var basePrice = BasePricePerTon;
			Interest = interest;
			PricePerTon = Tools.AddPercent(basePrice, interest);
		}

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
