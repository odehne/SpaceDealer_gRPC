using Cope.SpaceRogue.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Models
{

	public class ShipModel
	{
		public Guid ShipId { get; set; }
		public string Name { get; set; }
		public string PlayerName { get; set; }
		public int SpeedValue { get; set; }
		public int AttackValue { get; set; }
		public int DefenceValue { get; set; }
		public int SensorRangeValue { get; set; }
		public int HullValue { get; set; }
		public int ShieldsValue { get; set; }
		public string[] FeatureNames { get; set; }
		public Position CurrentSector { get; set; }
	}

}
