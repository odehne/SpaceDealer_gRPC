using Cope.SpaceRogue.Galaxy.API.Domain.SeedWork;
using Cope.SpaceRogue.Galaxy.API.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cope.SpaceRogue.Galaxy.API.Domain
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

		public EntityId ShipId { get; init; }
		public EntityId PlayerId { get; init; }
		public int Hull { get; set; }
		public int Shields { get; set; }

		public double LoadedCapacity { get; set; }
		public List<Feature> Features { get; set; }
		public List<Payload> Cargo { get; set; }
		
		public Ship(EntityId shipId, EntityId playerId)
		{
			ShipId = shipId;
			PlayerId = playerId;
			Cargo = new List<Payload>();
			Features = new List<Feature>();
		}

		public int GetAttackValue()
		{
			var modifier = Features.Sum(x => x.AttackModifier);
			return BASE_ATTACK_VALUE + modifier;
		}

		public int GetDefenceValue()
		{
			var modifier = Features.Sum(x => x.DefenceModifier);
			return BASE_DEFENCE_VALUE + modifier;
		}

		public int GetSpeedValue()
		{
			var modifier = Features.Sum(x => x.SpeedModifier);
			return BASE_SPEED_VALUE + modifier;
		}

		public int GetMaxHull()
		{
			var modifier = Features.Sum(x => x.HullModifier);
			return BASE_HULL_VALUE + modifier;
		}

		public int GetMaxShields()
		{
			var modifier = Features.Sum(x => x.ShieldModifier);
			return BASE_SHIELD_VALUE + modifier;
		}

		public double GetMaxCapacity()
		{
			var modifier = Features.Sum(x => x.CapacityModifier);
			return BASE_CARGO_CAPACITY_VALUE + modifier;
		}

		public ShipStates AddFeature(Feature feature)
		{
			if(feature==null)
				throw new InvalidEntityStateException(this, $"Feature cannot be null.");
			var f = Features.FirstOrDefault(x => x.FeatureId.Equals(feature.FeatureId));
			if (f != null)
			{
				throw new InvalidEntityStateException(this, $"Feature {f.FeatureId} added already.");
			}
			else
			{
				Features.Add(feature);
				return ShipStates.FeatureAdded;
			}
		}

		public ShipStates RemoveFeature(EntityId featureId)
		{
			if (featureId == default)
				throw new InvalidEntityStateException(this, $"FeatureId not found.");
			var f = Features.FirstOrDefault(x => x.FeatureId.Equals(featureId));
			if (f == null)
			{
				throw new InvalidEntityStateException(this, $"FeatureId {featureId} not found.");
			}
			else
			{
				Features.Remove(f);
				return ShipStates.FeatureRemoved;
			}
		}

		public ShipStates LoadCargo(EntityId payloadId, double quantity)
		{
			if (payloadId == default)
				throw new InvalidEntityStateException(this, $"Unknown payload.");

			if (!AllowedToLoad(payloadId, quantity))
			{
				return ShipStates.Overloaded;
			}
			else
			{
				return LoadCargoIntoShip(payloadId, quantity);
			}
		}

		private ShipStates LoadCargoIntoShip(EntityId payloadId, double quantity)
		{
			var product = Cargo.FirstOrDefault(x => x.ProductId.Equals(payloadId));
			if (product == null)
			{
				Cargo.Add(new Payload() { ProductId = payloadId, Quantity = quantity });
			}
			return ShipStates.CargoLoaded;
		}

		public ShipStates UnloadCargo(EntityId payloadId, double quantity)
		{
			if (payloadId == default)
				throw new InvalidEntityStateException(this, $"Unknown payload.");
			if(Cargo==null)
				throw new InvalidEntityStateException(this, $"No cargo.");
			if (NotLoaded(payloadId))
				throw new InvalidEntityStateException(this, $"{payloadId} Not loaded.");
			if (GetCurrentlyLoadedCapacity(payloadId) < quantity)
				throw new InvalidEntityStateException(this, $"Not enough loaded.");

			return UnloadCargoFromShip(payloadId, quantity);
		}

		private ShipStates UnloadCargoFromShip(EntityId payloadId, double quantity)
		{
			var product = Cargo.FirstOrDefault(x => x.ProductId.Equals(payloadId));
			product.Quantity -= quantity;
			return ShipStates.CargoUnloaded;
		}

		private bool AllowedToLoad(EntityId payloadId, double quantity)
		{
			if (GetTotalLoadedQuantity() + quantity > GetMaxCapacity())
				return false;
			return true;
		}


		private double GetTotalLoadedQuantity()
		{
			return Cargo.Sum(x => x.Quantity);
		}

		private bool NotLoaded(EntityId payloadId)
		{
			var p = Cargo.FirstOrDefault(x => x.ProductId.Equals(payloadId));
			return p != null;
		}

		private double GetCurrentlyLoadedCapacity(EntityId payloadId)
		{
			return Cargo.FirstOrDefault(x=>x.ProductId.Equals(payloadId)).Quantity;
		}

		public ShipStates RepairHull()
		{
			if (Hull < GetMaxHull())
			{
				Hull++;
				return ShipStates.HullRepaired;
			}
			return ShipStates.OK;
		}

		public ShipStates RepairShields()
		{
			if (Shields < GetMaxShields())
			{
				Shields++;
				return ShipStates.ShieldsRepaired;
			}
			return ShipStates.OK;
		}

		public ShipStates TakeHit()
		{
			return DecrementShieldValue();
		}

		private ShipStates DecrementShieldValue()
		{
			if (Shields > 0)
			{
				Shields--;
				return ShipStates.ShieldsDamaged;
			}
			else
			{
				return DecrementHull();
			}
		}

		private ShipStates IncrementHull()
		{
			if (Hull > 0)
			{
				Hull--;
				return ShipStates.HullDamaged;
			}
			else
			{
				return ShipStates.Destroyed;
			}
		}


		private ShipStates DecrementHull()
		{
			if (Hull > 0)
			{
				Hull--;
				return ShipStates.HullDamaged;
			}
			else
			{
				return ShipStates.Destroyed;
			}
		}

		protected override void EnsureValidState()
		{
			var valid = ShipId != default & PlayerId != default;
			if (!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}

}
