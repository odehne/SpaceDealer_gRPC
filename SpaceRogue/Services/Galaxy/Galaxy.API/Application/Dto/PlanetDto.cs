using Cope.SpaceRogue.Galaxy.API.Model;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class PlanetDto
	{
		public string ID { get; private set; }
		public string Name { get; private set; }
		public int PosX { get; private set; }
		public int PosY { get; private set; }
		public int PosZ { get; private set; }
		public string MarketPlaceId { get; private set; }

    }
}
