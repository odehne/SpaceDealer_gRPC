
using Cope.SpaceRogue.InfraStructure;
using Microsoft.Extensions.Logging;
using System;

namespace Cope.SpaceRogue.Travelling.API.Models
{
	public class PlanetModel
	{
		public Guid PlanetId { get; set; }
		public string Name { get; set; }
		public Position Sector { get; set; }
	}

}