
using Cope.SpaceRogue.InfraStructure;
using Cope.SpaceRogue.Maintenance.API.Domain.SeedWork;

namespace Cope.SpaceRogue.Maintenance.API.Domain
{
	public class Feature : Entity
	{
		public Entity FeatureId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int SpeedModifier { get; set; }
		public int AttackModifier { get; set; }
		public int DefenceModifier { get; set; }
		public int SensorModifier { get; set; }
		public int ShieldModifier { get; set; }
		public int HullModifier { get; set; }
		public double CapacityModifier { get; set; }

		protected override void EnsureValidState()
		{
			var valid = !string.IsNullOrEmpty(Name) && FeatureId != default;
			if(!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}