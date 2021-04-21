
using Cope.SpaceRogue.Infrastructure.Domain;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
    public class PlayerDto
	{
		public string ID { get; private set; }
		public string Name { get; private set; }
	
		public IEnumerable<string> ShipIds { get; private set; }

    }
}
