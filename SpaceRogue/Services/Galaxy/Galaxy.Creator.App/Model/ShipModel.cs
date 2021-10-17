using System.Collections.Generic;

namespace Galaxy.Creator.App.Model
{
	public class ShipModel
	{
		public enum ShipTypes
		{
			CargoVessel,
			PirateShip,
			Satelite,
			Freighter,
			Warship,
			SpaceStation,
			Drone,
			Probe
		}

		public string Id { get; set; }
		public string Name { get; set; }
		public string PlayerId { get; set; }

		public int Hull { get; set; }
		public int Shields { get; set; }
		public ShipTypes ShipType { get; set; }

		public List<FeatureModel> Features { get; set; }

		public ShipModel()
		{
			Features = new List<FeatureModel>();
		}

		public override string ToString()
		{
			return $"{Name}";
		}
	}

}
