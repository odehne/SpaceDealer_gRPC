namespace Cope.SpaceRogue.Galaxy.Creator.Application.Commands
{
	public class MarketPlaceDTO
	{
		public string MarketPlaceId { get; private set; }
		public string PlanetId { get; private set; }
		public string Name { get; set; }

		public MarketPlaceDTO(string marketPlaceId, string planetId, string name)
		{
			MarketPlaceId = marketPlaceId;
			PlanetId = planetId;
			Name = name;
		}
	}
}
