using SpaceDealer.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SpaceDealerModels.Units
{
	public class Ships : List<DbShip>
	{
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, DbCoordinates newPosition, DbShip ship);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, DbShip ship, DbCoordinates newPosition);

		[JsonIgnore]
		public DbPlayer Parent { get; set; }

		public Ships()
		{
		}

		public Ships(DbPlayer parent)
		{
			Parent = parent;
		}

		public DbShip GetShipByName(string name)
		{
			return this.FirstOrDefault(x => x.Name.Equals(name));
		}
		public DbShip GetShipInSector(DbCoordinates coordinates)
		{
			return this.FirstOrDefault(x => x.Cruise.CurrentSector.X == coordinates.X & 
											x.Cruise.CurrentSector.Y == coordinates.Y & 
											x.Cruise.CurrentSector.Z == coordinates.Z);
		}

		public bool AddShip(DbShip newShip)
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

		private void NewShip_Arrived(string message, DbCoordinates newPosition, DbShip ship)
		{
			Arrived?.Invoke(message, newPosition, ship);
		}

		private void NewShip_Interrupted(InterruptionType interruptionType, string message, DbShip ship, DbCoordinates newPosition)
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
