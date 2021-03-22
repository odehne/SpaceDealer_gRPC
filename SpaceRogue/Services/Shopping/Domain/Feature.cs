using Cope.SpaceRogue.Galaxy.API.Domain.SeedWork;
using Cope.SpaceRogue.Galaxy.API.InfraStructure;

namespace Cope.SpaceRogue.Galaxy.API.Domain
{
	public class Feature : Entity
	{
		public EntityId FeatureId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double BattleAdvantage { get; set; }
		public double BattleDisadvantage { get; set; }
		public double FreightCapacityAdvantage { get; set; }
		public double FreightCapacityDisadvantage { get; set; }
		public double SensorRangeAdvantage { get; set; }

		protected override void EnsureValidState()
		{
			var valid = !string.IsNullOrEmpty(Name) && FeatureId != default;
			if(!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}