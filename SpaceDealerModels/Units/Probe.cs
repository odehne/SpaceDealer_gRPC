using SpaceDealer.Enums;
using SpaceDealerModels.Events;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SpaceDealerModels.Units
{
    public enum ProbeStates
    {
        InActive,
        Travelling,
        SignalLost
    }

    public class Probe : BaseUnit
	{
        [XmlIgnore]
        [JsonIgnore]
        public Probes Parent { get; set; }
		public bool Released { get; set; }

        public string ShipId { get; set; }

        public ProbeStates State { get; set; }

		public DbCoordinates CurrentSector { get; set; }

        public bool Destroyed => State == ProbeStates.SignalLost;

        public Probe()
		{

		}

		public Probe(Probes parent)
		{
			Parent = parent;
			CurrentSector = Parent.Parent.Cruise.CurrentSector;
            ShipId = Parent.Parent.Id;
            Name = GetNewProbeName();
            State = ProbeStates.InActive;
		}

        public const int MaxRange = 200;

        public ProbeStates Eject()
        {
            if (State == ProbeStates.InActive)
            {
                // start at the ship's current position (own copy, so the probe doesn't move the ship)
                var shipSector = Parent.Parent.Cruise?.CurrentSector;
                if (shipSector != null)
                    CurrentSector = new DbCoordinates(shipSector.X, shipSector.Y, shipSector.Z);
                State = ProbeStates.Travelling;
            }

            return State;
        }


        // a probe can at max fly 200 squares without losing connection to the mothership
        // if the distance is greater the mother ship loses the signal to the probe
        public ProbeStates Update()
        {
            if (State == ProbeStates.InActive || State == ProbeStates.SignalLost)
                return State;

            var ship = Parent.Parent;
            var shipSector = ship.Cruise?.CurrentSector;
            if (shipSector == null || CurrentSector == null)
                return State;

            if (Math.Abs(CurrentSector.X - shipSector.X) >= MaxRange ||
                Math.Abs(CurrentSector.Y - shipSector.Y) >= MaxRange ||
                Math.Abs(CurrentSector.Z - shipSector.Z) >= MaxRange)
            {
                State = ProbeStates.SignalLost;
                var events = Parent.Parent.Parent.Parent.Parent.Events;   // same chain as KnownUniverse
                events.Publish(new ProbeSignalLostEvent(InterruptionType.ProbeScanned,
                    $"Das Raumschiff [{ship.Name} hat den Kontakt zu seiner Sonde {Name} gerade verloren. Die Sonde hat sich zu weit entfernt.", 
                    this, ship, ship.Parent.Parent));
            }
            else
            {
                CurrentSector = new DbCoordinates(CurrentSector.X + 1, CurrentSector.Y + 1, CurrentSector.Z + 1);
                var scanResult = ScanPlanetsInNearbySectors(ship, 1);

                if (scanResult.ScannedPlanets.Count > 0 || scanResult.ScannedShips.Count > 0)
                {
                    var events = Parent.Parent.Parent.Parent.Parent.Events;   // same chain as KnownUniverse
                    events.Publish(new AreaScannedEvent(InterruptionType.ProbeScanned,
                        $"Scanned area at {CurrentSector}. Found {scanResult.ScannedPlanets.Count} planets and {scanResult.ScannedShips.Count} ships.", 
                        this, ship, ship.Parent.Parent, scanResult.ScannedPlanets, scanResult.ScannedShips));
                }

            }

            return State;

        }

        public ScanResult ScanPlanetsInNearbySectors(DbShip ship, int layer = 1)
        {
            var nearbyPlanets = new Planets();
            var nearbyShips = new Ships();
            var sector = CurrentSector;
            var planetsInSector = new Planets();

            // Anzahl der Würfel = (2n+1)^3, wobei n der Scanbereich ist. Für n=1 ergibt sich (2*1 + 1)^3 = 27 Würfel.
            // Anzahl der Würfel pro layer = 2n+1.

            var size = 2 * layer + 1;

            for (int z = -layer; z <= layer; z++)
            {
                for (int y = -layer; y <= layer; y++)
                {
                    for (int x = -layer; x <= layer; x++)
                    {
                        // we don't want to scan the current sector, so we skip it
                        if (x == 0 && y == 0 && z == 0)
                            continue;

                        var scanSector = new DbCoordinates(sector.X + x, sector.Y + y, sector.Z + z);
                        var planetsInThisSector = Parent.Parent.Parent.Parent.Galaxy.GetPlanetsInSector(scanSector);
                        var shipsInSector = Parent.Parent.Parent.Parent.Parent.ScanShipsInSector(scanSector);

                        if (planetsInThisSector != null)
                        {
                            foreach (var planetInSector in planetsInThisSector.Values)
                            {
                                nearbyPlanets.TryAdd(planetInSector.Id, planetInSector);
                            }
                        }

                        if (shipsInSector != null)
                        {
                            foreach (var shipInSector in shipsInSector.Values)
                            {
                                if (shipInSector.Id != ship.Id) // Exclude the scanning ship itself
                                {
                                    nearbyShips.TryAdd(shipInSector.Id, shipInSector);
                                }
                            }
                        }

                    }
                }
            }

            return new ScanResult
            {
                ScannedPlanets = nearbyPlanets,
                ScannedShips = nearbyShips
            };
        }

        public static string GetNewProbeName()
        {
            return Guid.NewGuid().ToString()[9..18];
        }

    }
}
