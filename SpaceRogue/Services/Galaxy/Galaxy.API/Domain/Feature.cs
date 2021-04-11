
using Cope.SpaceRogue.Galaxy.API.Domain.SeedWork;
using Cope.SpaceRogue.InfraStructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cope.SpaceRogue.Galaxy.API.Domain
{
	public class Feature : Entity
	{
		[Key]
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double BattleAdvantage { get; set; }
		public double BattleDisadvantage { get; set; }
		public double FreightCapacityAdvantage { get; set; }
		public double FreightCapacityDisadvantage { get; set; }
		public double SensorRangeAdvantage { get; set; }

		public Feature()
		{
			ID = Guid.NewGuid();
		}

        public Feature(string name, string description, double battleAdvantage, double battleDisadvantage, double freightCapacityAdvantage, double freightCapacityDisadvantage, double sensorRangeAdvantage)
        {
            ID = Guid.NewGuid();
            Name = name;
            Description = description;
            BattleAdvantage = battleAdvantage;
            BattleDisadvantage = battleDisadvantage;
            FreightCapacityAdvantage = freightCapacityAdvantage;
            FreightCapacityDisadvantage = freightCapacityDisadvantage;
            SensorRangeAdvantage = sensorRangeAdvantage;
        }

        protected override void EnsureValidState()
		{
			var valid = !string.IsNullOrEmpty(Name) && ID != default;
			if(!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}