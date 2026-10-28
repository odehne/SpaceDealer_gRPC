using System.Text.Json.Serialization;
using SpaceDealer.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace SpaceDealerModels.Units
{

    public class DbShip : BaseUnit
	{
		[XmlIgnore]
        [JsonIgnore]
        public Ships Parent { get; set; }

        [XmlIgnore]
		public DbJourney Cruise { get; set; }

		//[JsonPropertyName("cargoSize")]
		//public double CargoSize { get; set; } // in tons
		[JsonPropertyName("cargo")]
		public CargoBay CargoBay { get; set; } // in tons

        [JsonPropertyName("features")]
		public DbFeatures Features {get; set;}

		[JsonPropertyName("state")]
		public ShipState State { get; set; }
		
		[JsonPropertyName("shields")]
		public int Shields { get; set; }

		[JsonPropertyName("hull")]
		public int Hull { get; set; }
		
		[JsonPropertyName("rangeBonus")]
		public int RangeBonus { get; set; }
		[JsonPropertyName("attackBonus")]
		
		public int AttackBonus { get; set; }
		[JsonPropertyName("defenceBonus")]
		public int DefenceBonus { get; set; }
		
		[JsonPropertyName("currentPlanet")]
		public DbPlanet CurrentPlanet { get; set; }

		public Probes Probes { get; set; }

		public const int DEFAULT_MAX_HULL_MID_SIZE_FREIGHTER = 3;
        public const int DEFAULT_MAX_SHIELDS_MID_SIZE_FREIGHTER = 2;
        public const int DEFAULT_CARGO_MID_SIZE_FREIGHTER = 30;
        public const double NEW_SHIP_COSTS = 150000.0;

        /// <summary>Installs a bought feature on this ship and applies its bonus.</summary>
        public void InstallFeature(DbFeature feature)
        {
            if (!Enum.TryParse<FeatureNames>(feature.Name, true, out var name))
                throw new ArgumentException($"Unknown feature [{feature.Name}].");

            // AdditionalProbe may be bought repeatedly, so it gets its own entry in the feature list.
            if (name != FeatureNames.AdditionalProbe)
                Features.Add(feature);

            switch (name)
            {
                case FeatureNames.ExtraShield:
                    Shields += feature.BonusValue;
                    DefenceBonus += feature.BonusValue;
                    break;
                case FeatureNames.ExtraPhasers:
                case FeatureNames.ExtraTorpedos:
                    AttackBonus += feature.BonusValue;
                    break;
                case FeatureNames.SignalRange_1:
                case FeatureNames.SignalRange_2:
                    RangeBonus += feature.BonusValue;
                    break;
                case FeatureNames.AdditionalCargoSpace_30:
                case FeatureNames.AdditionalCargoSpace_80:
                    CargoSize += feature.BonusValue;
                    break;
                case FeatureNames.AdditionalProbe:
                    var probe = new Probe(Probes);
                    Probes.TryAdd(probe.Id, probe);
                    break;
                // AdvancedWarp_1: no speed model yet, the feature is only recorded.
            }
        }

        public DbShip(string name, DbPlayer player, DbFeatures featureSet, Ships parent) : base(name)
		{
			State = ShipState.Idle;
			Shields = DEFAULT_MAX_SHIELDS_MID_SIZE_FREIGHTER;
			CargoSize = DEFAULT_CARGO_MID_SIZE_FREIGHTER;
            Hull = DEFAULT_MAX_HULL_MID_SIZE_FREIGHTER;
			Features = featureSet;
            Parent = parent;
            CargoBay = new CargoBay();
			CurrentPlanet = player.HomePlanet;
            PlayerId = player.Id;
            
			Cruise = new DbJourney(player.HomePlanet, player.HomePlanet, player.HomePlanet.Sector, this);
            
			Probes = new Probes(this);
			var probe = new Probe(Probes);
            Probes.TryAdd(probe.Id, probe);

  	}

		public void StartCruise(DbPlanet source, DbPlanet destination)
		{
			Cruise = new DbJourney(source, destination, source.Sector, this);
	    }

        public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public Result Load(DbProductsInStock productsToLoad)
		{
			if (State != ShipState.Idle && State != ShipState.Trading)
			{
				return new Result(ResultState.Failed, "Das Schiff kann momentan nicht beladen werden.");
			}
			if (productsToLoad.GetTotalWeight() > CargoSize - CargoBay.TotalQuantity)
			{
				return new Result(ResultState.Failed, $"Das Schiff kann nur maximal {CargoSize} tons aufnehmen.");
			}

            foreach (var product in productsToLoad)
            {
                CargoBay.AddCargo(this, product, (int)product.GetTotalWeight());
            }

			return new Result(ResultState.OK, $"Das Schiff wurde beladen. Verfügbarer Speicher {CargoSize - CargoBay.TotalQuantity} tons.");
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

			if (CargoBay != null)
			{
				foreach (var load in CargoBay)
				{
					load.Update();
				}
			}
			//Evaluate current position
			if (Cruise!=null)
				Cruise.Update();

			if(Probes!=null)
			{
				foreach (var probe in Probes.Values)
				{
					probe.Update();
				}
			}
		}

		public ScanResult ScanSpace()
		{
			var result = new ScanResult();

			var feat = Features.GetFeatureById("");

			return result;
		}

		public BattleResult Attack()
		{
			if(Cruise.EnemyBattleShip==null)
				return new BattleResult { Message = "Schiff ist entkommen." };

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
					return new BattleResult { CriticalHit = true, DefenderWasHit = false, Value = 1, Message = $"Oops! {Name}s Angriff schlug fehl und verusachte einen eigenen Schaden!" };
				if (attackRoll == 20)
					return new BattleResult { CriticalHit = true, DefenderWasHit = true, Value = 2, Message = $"Volltreffer! Exzellenter Angriff von {Name}, {enemy.Name} erhält doppelten Schaden!" };
			}
			else
			{
				attackRoll = enemy.AttackRoll();
				defenceRoll = SimpleDiceRoller.Roll(DiceType.d20, AttackBonus);
				//Criticals
				if (attackRoll == 2)
					return new BattleResult { CriticalHit = true, DefenderWasHit = false, Value = 1, Message = $"Oops! Da hat sich {enemy.Name} aber selbst ins Bein geschossen erhält einen Schaden!" };
				if (attackRoll == 12)
					return new BattleResult { CriticalHit = true, DefenderWasHit = true, Value = 2, Message = $"Hinterhältiger Angriff von {enemy.Name}. Die {Name} muss einen kritischen Schaden einstecken!" };
			}

			if (attackRoll <= defenceRoll)
				return new BattleResult { CriticalHit = false, DefenderWasHit = true, Value = 0, Message = $" konnte dem Angriff ausweichen!" };

			return new BattleResult { CriticalHit = false, DefenderWasHit = true, Value = 1, Message = "Das verteidigende Schiff wurde getroffen!" };
		}

	}
}
