using System;

namespace Cope.SpaceRogue.Shopping.API.Models
{
	public class ProductModel
	{
		public Guid ID { get; set; }
		public Guid GroupId { get; set; }
		public string Name { get; set; }
		public double PricePerUnit { get; set; }
		public double Rarity { get; set; }
		public double SizeInUnits { get; set; }
	
	}
}