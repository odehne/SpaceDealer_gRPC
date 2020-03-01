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
		public string Name { get; set; }
		public string Description { get; set; }
		public int AttackBonus { get; set; }
		public int DefenceBonus{ get; set; }
		public int RangeBonus { get; set; }
		public int SpeedBonus { get; set; }

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