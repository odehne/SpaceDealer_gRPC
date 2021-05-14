using System;

namespace Cope.SpaceRogue.Shopping.API.Models
{
	public class FeatureModel
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double FreightCapacityAdvantage { get; set; }
		public double FreightCapacityDisadvantage { get; set; }
	}
}