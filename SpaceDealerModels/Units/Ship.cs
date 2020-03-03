using SpaceDealer.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SpaceDealerModels.Units
{
	public class Ship : BaseUnit
	{
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, Coordinates newPosition, Ship ship);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, Ship ship, Coordinates newPosition);

		public Journey Cruise { get; set; }
		public double CargoSize { get; set; } // in tons
		public ProductsInStock CurrentLoad { get; set; }
		public ShipFeatures Features {get; set;}
		public Ships Parent { get; set; }
		public ShipState State { get; set; }
		public int Shields { get; set; }
		public int Hull { get; set; }
		public int RangeBonus { get; set; }
		public int AttackBonus { get; set; }
		public int DefenceBonus { get; set; }
		public Planet CurrentPlanet { get; set; }

		public Ship(string name, Planet homeplanet, ShipFeatures featureSet) : base(name)
		{
			State = ShipState.Idle;
			Shields = 2;
			Hull = 3;
			Features = featureSet;
			CurrentLoad = new ProductsInStock();
			CurrentPlanet = homeplanet;
			//Cruise = new Journey(homeplanet, homeplanet, homeplanet.Sector, this);
		}

		public void StartCruise(Planet source, Planet destination)
		{
			Cruise = new Journey(source, destination, source.Sector, this);
			Cruise.Arrived += Cruise_Arrived;
			Cruise.Interrupted += Cruise_Interrupted;
		}

		private void Cruise_Interrupted(InterruptionType interruptionType, string message, Coordinates newPosition)
		{
			Interrupted?.Invoke(interruptionType, message, this, newPosition); ;
		}

		private void Cruise_Arrived(string message, Coordinates newPosition)
		{
			CurrentPlanet = Cruise.Destination;
			Arrived?.Invoke(message, newPosition, this);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public Result Load(ProductsInStock productsToLoad)
		{
			if (State != ShipState.Idle)
			{
				return new Result(ResultState.Failed, "Das Schiff kann momentan nicht beladen werden.");
			}
			if(productsToLoad.GetTotalWeight() > CargoSize - CurrentLoad.GetTotalWeight())
			{
				return new Result(ResultState.Failed, $"Das Schiff kann nur maximal {CargoSize} tons aufnehmen.");
			}
			CurrentLoad.AddRange(productsToLoad);
			return new Result(ResultState.OK, $"Das Schiff wurde beladen. Verfügbarer Speicher {CargoSize - CurrentLoad.GetTotalWeight()} tons.");
		}

		public override string ToString()
		{
			if (Cruise != null)
			{
				var distance = Cruise.CurrentDistanceToDestination.ToString("0.##", CultureInfo.InvariantCulture);
				return $"Name: {Name}\tPosition: {Cruise.CurrentSector.ToString()}\t" +
					$"Ziel: {Cruise.Destination.Sector.ToString()}\tEntfernung:{distance} parsec";
			}

			return $"{Name} noch im Raumhafen.";
		}

		public override void Update()
		{
			base.Update();
			foreach (var load in CurrentLoad)
			{
				load.Update();
			}
			//Evaluate current position
			if(Cruise!=null)
				Cruise.Update();
		}

		public BattleResult Attack()
		{
			var result = Battle(true, Cruise.EnemyBattleShip);
			if (result.CriticalHit)
			{
				if (result.DefenderWasHit == false)
				{
					return ApplyDamage(result);
				}
			}
			if(result.DefenderWasHit)
				return Cruise.EnemyBattleShip.ApplyDamage(result);
			return result;
		}

		public BattleResult Defend()
		{
			var result = Battle(false, Cruise.EnemyBattleShip);
			if (result.CriticalHit)
			{
				if (result.DefenderWasHit == false)
				{
					return Cruise.EnemyBattleShip.ApplyDamage(result);
				}
			}
			if (result.DefenderWasHit)
				return ApplyDamage(result);
			return result;
		}

		private BattleResult ApplyDamage(BattleResult result)
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
					result.Message = "Das Schiff wurde beim Angriff zerstört.";
				}
			}
			return result;
		}

		private BattleResult Battle(bool attack, PirateShip enemy)
		{
			int attackRoll;
			int defenceRoll;
			if (attack)
			{
				attackRoll = SimpleDiceRoller.Roll(DiceType.d20, AttackBonus);
				defenceRoll = enemy.DefenceRoll();
				//Criticals
				if (attackRoll == 1)
					return new BattleResult { CriticalHit = true, DefenderWasHit = false, Value = 1, Message = "Oops! Angriff schlug fehl und verusachte einen eigenen Schaden!" };
				if (attackRoll == 20)
					return new BattleResult { CriticalHit = true, DefenderWasHit = true, Value = 2, Message = "Volltreffer! Exzellenter Angriff, der Gegner erhält doppelten Schaden!" };
			}
			else
			{
				attackRoll = enemy.AttackRoll();
				defenceRoll = SimpleDiceRoller.Roll(DiceType.d20, AttackBonus);
				//Criticals
				if (attackRoll == 2)
					return new BattleResult { CriticalHit = true, DefenderWasHit = false, Value = 1, Message = "Oops! Der Angriff schlug fehl und verursachte einen eigenen Schaden!" };
				if (attackRoll == 12)
					return new BattleResult { CriticalHit = true, DefenderWasHit = true, Value = 2, Message = "Volltreffer! Exzellenter Angriff, der Gegner erhält kritischen Schaden!" };
			}

			if (attackRoll <= defenceRoll)
				return new BattleResult { CriticalHit = false, DefenderWasHit = true, Value = 0, Message = "Das verteidigende Schiff konnte dem Angriff ausweichen!" };

			return new BattleResult { CriticalHit = false, DefenderWasHit = true, Value = 1, Message = "Das verteidigende Schiff wurde getroffen!" };
		}

	}
}
