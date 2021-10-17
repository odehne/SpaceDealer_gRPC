namespace Galaxy.Creator.App.Model
{
	public class FeatureModel
	{
		public string ID { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		public double BattleAdvantage { get; private set; }
		public double BattleDisadvantage { get; private set; }
		public double FreightCapacityAdvantage { get; private set; }
		public double FreightCapacityDisadvantage { get; private set; }
		public double SensorRangeAdvantage { get; private set; }

		public FeatureModel(string iD, string name, string description, double battleAdvantage, double battleDisadvantage, double freightCapacityAdvantage, double freightCapacityDisadvantage, double sensorRangeAdvantage)
		{
			ID = iD;
			Name = name;
			Description = description;
			BattleAdvantage = battleAdvantage;
			BattleDisadvantage = battleDisadvantage;
			FreightCapacityAdvantage = freightCapacityAdvantage;
			FreightCapacityDisadvantage = freightCapacityDisadvantage;
			SensorRangeAdvantage = sensorRangeAdvantage;
		}
	}

}
