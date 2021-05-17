using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cope.SpaceRogue.Infrastructure.Domain
{

	public class Ship : Entity
	{
		public enum ShipStates
		{
			OK,
			ShieldsDestroyed,
			ShieldsDamaged,
			ShieldsRepaired,
			HullDamaged,
			HullRepaired,
			CargoLoaded,
			CargoUnloaded,
			SensorRangeIncreased,
			AttackValueIncreased,
			AttackValueDecreased,
			DefenceValueIncreased,
			DefenceValueDecreased,
			ShipSpeedIncreased,
			FeatureAdded,
			FeatureRemoved,
			Overloaded,
			Destroyed
		}

		public const int BASE_DEFENCE_VALUE = 1;
		public const int BASE_ATTACK_VALUE = 1;
		public const int BASE_SPEED_VALUE = 1;
		public const int BASE_HULL_VALUE = 3;
		public const int BASE_SHIELD_VALUE = 3;
		public const int BASE_SENSOR_RANGE_VALUE = 1;
		public const double BASE_CARGO_CAPACITY_VALUE = 120.0;


		[Key]
		public Guid ID { get; set; }
		public int Hull { get; set; }
		public int Shields { get; set; }
		public string Name { get; set; }
		public Guid PlayerID { get; set; }
	
		public Ship()
		{
			Features = new List<Feature>();
			Cargo = new List<Payload>();
		}

        public Ship(string name, int hull, int shields)
        {
            ID = Guid.NewGuid();
		    Hull = hull;
            Shields = shields;
            Name = name;
        }

		public int GetAccumulatedAttackValue()
		{
			int value = BASE_ATTACK_VALUE;
			foreach (var ft in Features)
			{
				value += (int)ft.BattleAdvantage - (int)ft.BattleDisadvantage;
			}
			return value;
		}

		public string[] GetFeatureNames()
		{
			return Features.Select(x => x.Name).ToArray();
		}

		public int GetAccumulatedHullValue()
		{
			int value = BASE_HULL_VALUE;
			foreach (var ft in Features)
			{
				value += (int)ft.HullAdvantage;
			}
			return value;
		}

		public int GetSpeedValue()
		{
			int value = BASE_SPEED_VALUE;
			foreach (var ft in Features)
			{
				value += (int)ft.SpeedAdvantage - (int)ft.SpeedDisadvantage;
			}
			return value;
		}

		public int GetSensorRangeValue()
		{
			int value = BASE_SENSOR_RANGE_VALUE;
			foreach (var ft in Features)
			{
				value += (int)ft.SensorRangeAdvantage;
			}
			return value;
		}

		public int GetAccumulatedShieldsValue()
		{
			int value = BASE_SHIELD_VALUE;
			foreach (var ft in Features)
			{
				value += (int)ft.ShieldsAdvantage;
			}
			return value;
		}

		public int GetAccumulatedDefenceValue()
		{
			int defenceValue = BASE_DEFENCE_VALUE;
			foreach (var ft in Features)
			{
				defenceValue += (int)ft.DefenceAdvantage - (int)ft.DefenceDisadvantage;
			}
			return defenceValue;
		}

		public double LoadedCapacity { get; set; }
		public ICollection<Feature> Features { get; set; }
		public ICollection<Payload> Cargo { get; set; }


		protected override void EnsureValidState()
		{
			throw new NotImplementedException();
		}
	}
}