using Cope.SpaceRogue.Galaxy.Creator.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Creator.App.Model
{
	public class PlanetModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public Position Sector { get; set; }
		public string MarketId { get; set; }
	}

	public class ProductGroupModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
	}

	public class ProductModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public double Rarity { get; internal set; }
		public double Size { get; internal set; }
	}

	public class ShipModel
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public int Hull { get; set; }
		public int Shields { get; set; }

		public List<FeatureModel> Features { get; set; }

		public ShipModel()
		{
			Features = new List<FeatureModel>();
		}
	}

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

	public class PlayerModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<ShipModel> Ships { get; set; }
		public double Credits { get; set; }
	}

}
