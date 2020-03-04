using Newtonsoft.Json;
using SpaceDealer.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
	public class ShipFeatures : List<ShipFeature>
	{
		public ShipFeature GetFeatureByName(string name)
		{
			return this.FirstOrDefault(x => x.Name.Equals(name));
		}
	}

	public class ShipFeature 
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("description")]
		public string Description { get; set; }
		[JsonProperty("attackBonus")]
		public int AttackBonus { get; set; }
		[JsonProperty("defencBonus")]
		public int DefenceBonus{ get; set; }
		[JsonProperty("rangeBonus")]
		public int RangeBonus { get; set; }
		[JsonProperty("speedBonus")]
		public int SpeedBonus { get; set; }

		public ShipFeature()
		{
		}

		public ShipFeature(string name, string description, int attackBonus, int defenceBonus, int rangeBonus, int speedBonus)
		{
			Name = name;
			Description = description;
			AttackBonus= attackBonus;
			RangeBonus = rangeBonus;
			DefenceBonus = defenceBonus;
			SpeedBonus = speedBonus;
		}

		public override string ToString()
		{
			return base.ToString();
		}

	}
}