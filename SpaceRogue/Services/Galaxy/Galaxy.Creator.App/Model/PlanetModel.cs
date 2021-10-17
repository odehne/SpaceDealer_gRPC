using Cope.SpaceRogue.Infrastructure;

namespace Galaxy.Creator.App.Model
{
	public class PlanetModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public Position Sector { get; set; }
		public string MarketId { get; set; }

		public override string ToString()
		{
			return $"{Name} [{Sector.X},{Sector.Y},{Sector.Z}]";
		}
	}

}
