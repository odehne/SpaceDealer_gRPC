using System.Collections.Generic;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
    public class ShipDto
	{
		public string ID { get; set; }
		public string Name { get; set; }
		public int Hull { get; set; }
		public int Shields { get; set; }
		public List<FeatureDto> Features { get; set; }

	}
}
