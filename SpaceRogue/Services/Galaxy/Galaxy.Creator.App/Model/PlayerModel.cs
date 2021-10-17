using System.Collections.Generic;

namespace Galaxy.Creator.App.Model
{
	public class PlayerModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<ShipModel> Ships { get; set; }
		public double Credits { get; set; }
		public int PlayerType { get; set; }
		public PlanetModel HomePlanet { get; set; }
	}

}
