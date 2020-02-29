using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
	public class ShipFeatures : List<ShipFeature>
	{
		public ShipFeature GetFeatureByName(string name)
		{
			return this.FirstOrDefault(x => x.Name.Equals(name));
		}
	}

	public class AdvancedWarpFeature : ShipFeature
	{
		public double WarpFactor { get; set; }

		public AdvancedWarpFeature(string name, List<KeyValuePair<string, string>> properties) : base(name, properties)
		{
			WarpFactor = 1.2;
		}
	}

	public class ReplicatorFeature : ShipFeature
	{
		public bool Replicator { get; set; }

		public ReplicatorFeature(string name, List<KeyValuePair<string, string>> properties) : base(name, properties)
		{
			Replicator = true;
		}
	}

	public class AdvancedShieldsFeature : ShipFeature
	{
		public double ShieldFactor { get; set; }

		public AdvancedShieldsFeature(string name, List<KeyValuePair<string, string>> properties) : base(name, properties)
		{
			ShieldFactor = 1.5;
		}
	}

	public class SignalRangeFeature : ShipFeature
	{
		public int SectorRange { get; set; }

		public SignalRangeFeature(string name, List<KeyValuePair<string, string>> properties) : base(name, properties)
		{
			SectorRange = 1;
		}
	}

	public class ShipFeature : BaseUnit
	{

		public ShipFeature(string name, List<KeyValuePair<string, string>> properties) : base(name, properties)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override void Update()
		{
			base.Update();
		}
	}
}