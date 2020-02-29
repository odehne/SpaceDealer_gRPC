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
			Name = name;
			Sector = sector;
		}

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
		public HeavyCruiserPirateShip(string name, Coordinates sector, ShipFeatures featureSet)
		{
			if (featureSet == null)
			{
				Features = new ShipFeatures();
				Features.Add(new ShipFeature("Schilde", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Starke Verteidung", "Verteidigt mit d20") }));
				Features.Add(new ShipFeature("Erweitertes Waffenmodul", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Erweitertes Waffenmodul", "Angriff mit d20+3") }));
			}
			else
			{
				Features = featureSet;
			}
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
