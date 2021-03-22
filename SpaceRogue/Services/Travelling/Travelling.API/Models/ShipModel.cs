using Cope.SpaceRogue.InfraStructure;
using System;

namespace Cope.SpaceRogue.Traveling.API.Models
{
	public class ShipModel
	{
		public Guid ShipId { get; set; }
		public string Name { get; set; }
		public string PlayerName { get; set; }
		public Position AstronomicalPosition { get; set; }
		public int Speed { get; set; }
		public int SensorRange { get; set; }
	}
}