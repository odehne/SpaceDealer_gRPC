using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpaceDealerModels.Units
{

	public class Planet : BaseUnit
	{
		[JsonProperty("sector")]
		public Coordinates Sector { get; set; }
		[JsonProperty("market")]
		public Market Market { get; set; }
		[JsonProperty("industries")]
		public Industries Industries { get; set; }

		public Planet()
		{
		}

		public Planet(string name, Coordinates position, Market market, Industries industries) : base(name)
		{
			Sector = position;
			Market = market;
			Industries = industries;
		}

		public Planet(string name) : base(name)
		{
			Market = new Market($"{Name}.Market", this);
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
