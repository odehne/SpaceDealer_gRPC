using System.Collections.Generic;
using System.Linq;

namespace SpaceDealer.Units
{
	public class Ships : List<Ship>
	{
		public Player Parent { get; set; }

		public Ships(Player parent)
		{
			Parent = parent;
		}

		public Ship GetShipByName(string name)
		{
			return this.FirstOrDefault(x => x.Name.Equals(name));
		}
		public Ship GetShipInSector(Coordinates coordinates)
		{
			return this.FirstOrDefault(x => x.Cruise.CurrentSector.X == coordinates.X & 
											x.Cruise.CurrentSector.Y == coordinates.Y & 
											x.Cruise.CurrentSector.Z == coordinates.Z);
		}

		public bool AddShip(Ship newShip)
		{
			var p = GetShipByName(newShip.Name);
			if (p == null)
			{
				newShip.Parent = this;
				Add(newShip);
			}
			return false;
		}

		public override string ToString()
		{
			var ret = "";
			foreach (var ship in this)
			{
				ret += ship.ToString() + "\n";
			}
			return ret.TrimEnd('\n');
		}
	}
}
