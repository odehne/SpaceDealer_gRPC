using Cope.SpaceRogue.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cope.SpaceRogue.Galaxy.Creator.API.Domain
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
		public const double BASE_CARGO_CAPACITY_VALUE = 120.0;


		[Key]
		public Guid ID { get; set; }
		public int Hull { get; set; }
		public int Shields { get; set; }
		public string Name { get; set; }


		[ForeignKey("Player")]
		public virtual Player Owener { get; set; }

		public double LoadedCapacity { get; set; }
		public ICollection<Feature> Features { get; set; }
		public ICollection<Payload> Cargo { get; set; }


		protected override void EnsureValidState()
		{
			throw new NotImplementedException();
		}
	}
}