using SpaceDealer.Enums;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Xml.Serialization;

namespace SpaceDealerModels.Units
{
	public class Ships : ConcurrentDictionary<string, DbShip>
	{
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, DbCoordinates newPosition, DbShip ship);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, DbShip ship, DbCoordinates newPosition);
        public event AreaScanned Scanned;
        public delegate void AreaScanned(InterruptionType interruptionType, string message, DbShip ship, DbPlayer player, Planets scannedPlanets, Ships scannedShips);

        [XmlIgnore]
		[System.Text.Json.Serialization.JsonIgnore]
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
			var ship = Values.FirstOrDefault(x => x.Name == name);
            return ship;
        }
        public DbShip GetShipById(string uid)
        {
            return TryGetValue(uid, out var ship) ? ship : null;

        }

		public bool AddShip(DbShip newShip)
		{
			var p = GetShipByName(newShip.Name);
			if (p == null)
			{
				newShip.Parent = this;
				TryAdd(newShip.Id, newShip);
                newShip.CurrentPlanet = Parent.HomePlanet;
				var destination = Parent.GetRandomDiscoveredPlanet();
                newShip.Cruise = new DbJourney(newShip.CurrentPlanet, destination, newShip.CurrentPlanet.Sector, newShip);
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
