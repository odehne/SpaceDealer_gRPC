
using Cope.SpaceRogue.Infrastructure.Domain;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
    public class PlayerDto
	{
		public string ID { get; private set; }
		public string Name { get; private set; }
		public int PlayerType { get; private set; }
		public PlanetDto HomePlanet { get; private set; }

		public IEnumerable<string> ShipIds { get; private set; }

    }
}
