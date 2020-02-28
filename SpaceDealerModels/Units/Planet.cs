using System.Collections.Generic;

namespace SpaceDealerModels.Units
{

	public class Planet : BaseUnit
	{
		public Coordinates Sector { get; set; }
		public Market Market { get; set; }
		public Industries Industries { get; set; }

		public Planet(string name, List<KeyValuePair<string, string>> properties, Coordinates position, Market market, Industries industries) : base(name, properties)
		{
			Sector = position;
			Market = market;
			Industries = industries;
		}

		public Planet(string name, List<KeyValuePair<string, string>> properties) : base(name, properties)
		{
			Market = new Market($"{Name}.Market", new List<KeyValuePair<string, string>>(), this);
			Industries = new Industries(this);
		}

		public override void Update()
		{
			base.Update();
			Market.Update();
			foreach (var industry in Industries)
			{
				industry.Update();
			}
		}
		public override string ToString()
		{
			return $"{Name} {Sector.ToString()}"; //, Industriezweige: {Industries.ToString()}";
		}
	}
}
