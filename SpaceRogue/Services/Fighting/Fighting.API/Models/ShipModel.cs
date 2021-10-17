using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Domain;
using System;

namespace Cope.SpaceRogue.Fighting.API.Models
{
	public class ShipModel
	{
		public Guid ShipId { get; set; }
		public string Name { get; set; }
		public string PlayerName { get; set; }
		public int SpeedValue { get; set; }
		public int AttackValue { get; set; }
		public int DefenceValue { get; set; }
		public int SensorRangeValue { get; set; }
		public int HullValue { get; set; }
		public int ShieldsValue { get; set; }
		public string[] FeatureNames { get; set; }
		public Position CurrentSector { get; set; }
		public ShipStates State { get; set; }

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

		public static ShipModel MapTo(Ship entity)
		{
			return new ShipModel
			{
				ShipId = entity.Id,
				Name = entity.Name,
				CurrentSector = new Position(0, 0, 0),
				DefenceValue = entity.GetAccumulatedDefenceValue(),
				AttackValue = entity.GetAccumulatedAttackValue(),
				HullValue = entity.GetAccumulatedHullValue(),
				ShieldsValue = entity.GetAccumulatedShieldsValue(),
				SpeedValue = entity.GetSpeedValue(),
				SensorRangeValue = entity.GetSensorRangeValue(),
				FeatureNames = entity.GetFeatureNames()
			};
		}
	}

}
