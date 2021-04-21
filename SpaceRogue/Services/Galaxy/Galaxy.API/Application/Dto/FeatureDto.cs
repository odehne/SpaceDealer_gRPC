namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class FeatureDto
	{
		public string ID { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		public double BattleAdvantage { get; private set; }
		public double BattleDisadvantage { get; private set; }
		public double FreightCapacityAdvantage { get; private set; }
		public double FreightCapacityDisadvantage { get; private set; }
		public double SensorRangeAdvantage { get; private set; }

	}
}
