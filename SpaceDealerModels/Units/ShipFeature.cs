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