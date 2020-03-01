using SpaceDealerModels.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceDealerModels.Units
{
	public abstract class PirateShip
	{
		public ShipFeatures Features { get; set; }
		public string Name { get; set; }
		public Coordinates Sector { get; set; }
		public abstract int AttackRoll();
		public abstract int DefenceRoll();
		public abstract int Shields { get; set; }
		public abstract int Hull { get; set; }

		public BattleResult ApplyDamage(BattleResult result)
		{
			if (Shields > 0)
			{
				Shields -= 1;
			}
			else
			{
				if (Hull > 0)
				{
					Hull -= 1;
				}
				else
				{
					result.Message = "Das Piratenschiff wurde beim Angriff zerstört.";
				}
			}
			return result;
		}
	}

	public class SimplePirateShip : PirateShip
	{
		public SimplePirateShip(string name, Coordinates sector, ShipFeatures featureSet)
		{
			if (featureSet == null)
			{
				Features = new ShipFeatures();
			}
			else
			{
				Features = featureSet;
			}
			Shields = 0;
			Hull = 3;
			Name = name;
			Sector = sector;
		}

		public override int Shields { get; set; }
		public override int Hull { get; set; }

		public override int AttackRoll()
		{
			return SimpleDiceRoller.Roll(DiceType.d6) + SimpleDiceRoller.Roll(DiceType.d6);
		}

		public override int DefenceRoll()
		{
			return SimpleDiceRoller.Roll(DiceType.d6) + SimpleDiceRoller.Roll(DiceType.d6);
		}
	}

	public class CruiserPirateShip : PirateShip
	{
		public override int Shields { get; set; }
		public override int Hull { get; set; }

		public CruiserPirateShip(string name, Coordinates sector, ShipFeatures featureSet)
		{
			if (featureSet == null)
			{
				Features = new ShipFeatures();
			}
			else
			{
				Features = featureSet;
			}
			Shields = 0;
			Hull = 5;
			Name = name;
			Sector = sector;
		}

		public override int AttackRoll()
		{
			return SimpleDiceRoller.Roll(DiceType.d20);
		}

		public override int DefenceRoll()
		{
			return SimpleDiceRoller.Roll(DiceType.d6) + SimpleDiceRoller.Roll(DiceType.d6);
		}
	}

	public class HeavyCruiserPirateShip : PirateShip
	{
		public override int Shields { get; set; }
		public override int Hull { get; set; }

		public HeavyCruiserPirateShip(string name, Coordinates sector, ShipFeatures featureSet)
		{
			if (featureSet == null)
			{
				Features = new ShipFeatures();
				Features.Add(Repository.GetFeature("ShieldBonus+1"));
				Features.Add(Repository.GetFeature("Phasers+1"));
			}
			else
			{
				Features = featureSet;
			}
			Shields = 3;
			Hull = 5;
			Name = name;
			Sector = sector;
		}

		public override int AttackRoll()
		{
			return SimpleDiceRoller.Roll(DiceType.d20, 3);
		}

		public override int DefenceRoll()
		{
			return SimpleDiceRoller.Roll(DiceType.d20);
		}

	
	}
}
