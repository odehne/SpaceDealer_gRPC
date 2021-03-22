using SpaceDealerModels.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceDealerModels.Units
{
	public abstract class PirateShip
	{
		public DbFeatures Features { get; set; }
		public string Name { get; set; }
		public DbCoordinates Sector { get; set; }
		public abstract int AttackRoll();
		public abstract int DefenceRoll();
		public abstract int Shields { get; set; }
		public abstract int Hull { get; set; }
		public double Credits { get; set; }
		public string PicturePath { get; set; }
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
					result.Message = $"Hurra! Das Piratenschiff {Name} wurde beim Angriff zerstört. Credits verdient: {Credits}.";
					result.Treasure = Credits;
					result.Defeaded = true;
				}
			}
			return result;
		}
	}

	public class SimplePirateShip : PirateShip
	{
		public SimplePirateShip(string name, DbCoordinates sector, DbFeatures featureSet)
		{
			if (featureSet == null)
			{
				Features = new DbFeatures();
			}
			else
			{
				Features = featureSet;
			}
			Shields = 0;
			Hull = 3;
			Name = name;
			Sector = sector;
			PicturePath = ".\\Spaceships\\Contauri.jpg";
			Credits = SimpleDiceRoller.GetRandomCredits(1000, 5000);
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

		public CruiserPirateShip(string name, DbCoordinates sector, DbFeatures featureSet)
		{
			if (featureSet == null)
			{
				Features = new DbFeatures();
			}
			else
			{
				Features = featureSet;
			}
			Shields = 0;
			Hull = 5;
			Name = name;
			Sector = sector;
			PicturePath = ".\\Spaceships\\Dominion.jpg";
			Credits = SimpleDiceRoller.GetRandomCredits(5000, 15000);
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

		public HeavyCruiserPirateShip(string name, DbCoordinates sector, DbFeatures featureSet)
		{
			if (featureSet == null)
			{
				Features = new DbFeatures();
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
			PicturePath = ".\\Spaceships\\Praditor.jpg";
			Credits = SimpleDiceRoller.GetRandomCredits(50000, 150000);
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
