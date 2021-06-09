using Cope.SpaceRogue.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cope.SpaceRogue.Shopping.API.Models
{

	public class ShipModel
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public Guid PlayerId { get; set; }
		public Position CurrentSector { get; set; }
		public double Capacity { get; set; }
		public List<FeatureModel> Features { get; set; }
		public List<PayloadModel> Cargo { get; set; }

		public ShipModel()
		{
			Features = new List<FeatureModel>();
			Cargo = new List<PayloadModel>();
			CurrentSector = new Position(0, 0, 0);
		}

		public double TotalCapacity { 
			get 
			{
				double bonus = 0.0;
				foreach (var ft in Features)
				{
					bonus = bonus + ft.FreightCapacityAdvantage - ft.FreightCapacityDisadvantage;
				}
				return bonus;
			} 
		}

		internal bool EnoughLoadedToSell(string catalogItemId, double amount)
		{
			var loadedAmount = Cargo.Where(p => p.ID.Equals(catalogItemId)).Sum(x => x.Quantity); ;
			return loadedAmount >= amount;
		}
	}
}