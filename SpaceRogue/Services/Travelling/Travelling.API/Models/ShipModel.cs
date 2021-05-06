using Cope.SpaceRogue.InfraStructure;
using System;

namespace Cope.SpaceRogue.Travelling.API.Models
{
	public class ShipModel
	{
		public Guid ShipId { get; set; }
		public string Name { get; set; }
		public Guid PlayerId { get; set; }
		public Position CurrentSector { get; set; }
		public int Speed { get; set; }
		public int SensorRange { get; set; }
		public Position TargetSector { get; set; }
	}
}