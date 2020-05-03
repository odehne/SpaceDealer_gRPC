using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpaceDealerModels.Units
{
	public class DbPlanet : BaseUnit
	{
		[JsonProperty("sector")]
		public Coordinates Sector { get; set; }
		[JsonProperty("market")]
		public Market Market { get; set; }
		[JsonProperty("industries")]
		public DbIndustry Industry { get; set; }

		public DbPlanet()
		{
		}

		public DbPlanet(string name, Coordinates position, Market market, DbIndustry industry) : base(name)
		{
			Sector = position;
			Market = market;
			Industry = industry;
		}

		public DbPlanet(string name) : base(name)
		{
			Market = new Market($"{Name}.Market", this);
			Industry = new DbIndustry($"{Name}.Industry");
		}

		public override void Update()
		{
			base.Update();
			Market.Update();
			Industry.Update();
		}
		public override string ToString()
		{
			return $"{Name} {Sector.ToString()}"; //, Industriezweige: {Industries.ToString()}";
		}
	}
}
