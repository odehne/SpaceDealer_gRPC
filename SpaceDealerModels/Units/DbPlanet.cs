using System.Text.Json.Serialization;

namespace SpaceDealerModels.Units
{
	public class DbPlanet : BaseUnit
	{
		[JsonPropertyName("sector")]
		public DbCoordinates Sector { get; set; }
		//[JsonPropertyName("market")]
		//public DbMarket Market { get; set; }
		[JsonPropertyName("industries")]
		public DbIndustry Industry { get; set; }

		public DbPlanet()
		{
		}

		public DbPlanet(string name, DbCoordinates position, DbIndustry industry) : base(name)
		{
			Sector = position;
			Industry = industry;
		}

		public DbPlanet(string name) : base(name)
		{
			Industry = new DbIndustry($"{Name}.Industry");
		}

		public override void Update()
		{
			base.Update();
			//Market.Update();
			Industry.Update();
		}
		public override string ToString()
		{
			return $"{Name} {Sector.ToString()}"; //, Industriezweige: {Industries.ToString()}";
		}
	}
}
