using SpaceDealer.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
	public class Ships : List<Ship>
	{
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, Coordinates newPosition, Ship ship);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, Ship ship, Coordinates newPosition);


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
				newShip.Interrupted += NewShip_Interrupted;
				newShip.Arrived += NewShip_Arrived;
			}
			return false;
		}

		private void NewShip_Arrived(string message, Coordinates newPosition, Ship ship)
		{
			Arrived?.Invoke(message, newPosition, ship);
		}

		private void NewShip_Interrupted(InterruptionType interruptionType, string message, Ship ship, Coordinates newPosition)
		{
			Interrupted?.Invoke(interruptionType, message, ship, newPosition);
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
