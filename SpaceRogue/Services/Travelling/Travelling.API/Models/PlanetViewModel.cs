
using Cope.SpaceRogue.InfraStructure;
using System;

namespace Cope.SpaceRogue.Traveling.API.Models
{
	public class PlanetModel
	{
		public Guid PlanetId { get; set; }
		public string Name { get; set; }
		public Position AstronomicalPosition { get; set; }
	}
}