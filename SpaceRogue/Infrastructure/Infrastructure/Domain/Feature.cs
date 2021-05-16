
using Cope.SpaceRogue.Infrastructure.Domain.SeedWork;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cope.SpaceRogue.Infrastructure.Domain
{
	public class Feature : Entity
	{
		[Key]
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double BattleAdvantage { get; set; }
		public double BattleDisadvantage { get; set; }
		public double DefenceAdvantage { get; set; }
		public double DefenceDisadvantage { get; set; }
		public double FreightCapacityAdvantage { get; set; }
		public double FreightCapacityDisadvantage { get; set; }
		public double SensorRangeAdvantage { get; set; }
		public int HullAdvantage { get; set; }
		public int ShieldsAdvantage { get; set; }
		public int SpeedAdvantage { get; set; }
		public int SpeedDisadvantage { get; set; }


		public Feature()
		{
			ID = Guid.NewGuid();
		}

        public Feature(string name, string description, double battleAdvantage, 
						double battleDisadvantage, double defenceAdvantage, 
						double defenceDisadvantage, double freightCapacityAdvantage, 
						double freightCapacityDisadvantage, double sensorRangeAdvantage,
						int hullAdvantage, int shieldsAdvantage, int speedAdvantage, int speedDisadvantage)
        {
            ID = Guid.NewGuid();
            Name = name;
            Description = description;
            BattleAdvantage = battleAdvantage;
            BattleDisadvantage = battleDisadvantage;
			DefenceAdvantage = defenceAdvantage;
			DefenceDisadvantage= defenceDisadvantage;
			FreightCapacityAdvantage = freightCapacityAdvantage;
            FreightCapacityDisadvantage = freightCapacityDisadvantage;
            SensorRangeAdvantage = sensorRangeAdvantage;
			HullAdvantage = hullAdvantage;
			ShieldsAdvantage = shieldsAdvantage;
			SpeedDisadvantage = speedDisadvantage;
			SpeedAdvantage = speedAdvantage;
		}

        protected override void EnsureValidState()
		{
			var valid = !string.IsNullOrEmpty(Name) && ID != default;
			if(!valid)
				throw new InvalidEntityStateException(this, $"Postchecks failed.");
		}
	}
}