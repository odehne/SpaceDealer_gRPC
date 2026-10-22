using SpaceDealer.Enums;
using SpaceDealerModels.Events;
using SpaceDealerModels.Repositories;
using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SpaceDealerModels.Units
{
	public class DbJourney
	{
        [XmlIgnore]
		[JsonIgnore]
		public DbShip Parent { get; set; }
		[JsonPropertyName("departure")]
		public DbPlanet Departure { get; set; }
		[JsonPropertyName("detination")]
		public DbPlanet Destination { get; set; }
		[JsonPropertyName("currentSector")]
		public DbCoordinates CurrentSector { get; set; }
		[JsonPropertyName("destinationCoordinates")]
		public DbCoordinates DestinationCoordinates { get; set; }
		[JsonPropertyName("state")]
		JourneyState State { get; set; }
		[JsonPropertyName("enemyBattleShip")]
		public PirateShip EnemyBattleShip { get; set; }
		[JsonPropertyName("newlyDisoveredPlanet")]
		public DbPlanet DiscoveredPlanet { get; set; }
		
		public DbJourney()
		{

		}

	    public DbJourney(DbPlanet departure, DbPlanet destination, DbCoordinates position, DbShip parent)
        {
            Parent = parent;
            Departure = departure;
            Destination = destination;
            CurrentSector = position;
            
            if(CurrentSector.Equals(Destination.Sector))
                State = JourneyState.Arrived;
            else
                State = JourneyState.Travelling;

        }



        public double CurrentDistanceToDestination 
		{ 
			get
			{
				return DbCoordinates.GetDistanceLength(CurrentSector, Destination.Sector);
			}
		} // in parsec 3.26 Light years
		
		public void Update()
		{

            var knownUniverse = Parent.Parent.Parent.Parent.KnownUniverse;

            //

            if (State == JourneyState.NewPlanetInRange)
			{
                Console.WriteLine($"[RADIO] {Parent.Name} reported planet in range: {DiscoveredPlanet.Name} at {CurrentSector.ToString()}");
                State = JourneyState.Travelling;
            }
			
			if (State == JourneyState.Travelling)
			{
                // check if we have arrived at the destination
                if (CurrentSector.Equals(Destination.Sector))
				{
					CurrentSector = Destination.Sector;
					Departure = Destination;
					Parent.CurrentPlanet = Destination;
					State = JourneyState.Arrived;
                    var events = Parent.Parent.Parent.Parent.Events;
                    events.Publish(new JourneyArrivedEvent($"Arrived at destination [{Destination.Name}].", Parent.Parent.Parent, CurrentSector, Parent));
                }
				else
				{
                    // scann the area around the ship for planets and other ships
                    State = JourneyState.Travelling;
                    
                    knownUniverse.MoveShip(CurrentSector, Destination.Sector, Parent.Id);
                    CurrentSector = DbCoordinates.Move(CurrentSector, Destination.Sector);

                    var scanResult = ScanArea();
                    var scannedPlanets = scanResult.ScannedPlanets;
                    var scannedShips = scanResult.ScannedShips;

                    if (scannedPlanets.Count > 0 || scannedShips.Count > 0)
                    {
                        var events = Parent.Parent.Parent.Parent.Events;   // same chain as KnownUniverse
                        events.Publish(new AreaScannedEvent(InterruptionType.AreaScanned,
                            $"Scanned area at {CurrentSector}. Found {scannedPlanets.Count} planets and {scannedShips.Count} ships.",
                            Parent, Parent.Parent.Parent, scannedPlanets, scannedShips));
                    }
					else
					{
                        // nothing found, check for random interruptions
                        var interruption = CheckInterruptions();
                        if (interruption != null)
                        {
                            var events = Parent.Parent.Parent.Parent.Events;
                            events.Publish(new JourneyInterruptedEvent(interruption.Type, Parent.Parent.Parent, interruption.Message, Parent, CurrentSector));
                        }
                    }
				}
			}
		}

		public bool ContinueTravel()
		{
			if(State!=JourneyState.Travelling)
			{
				if(CurrentSector!=Destination.Sector)
				{
					State = JourneyState.Travelling;
					return true;
				}
			}
			return false;
		}

		private ScanResult ScanArea()
        {
            var scanSize = 1 + Parent.RangeBonus;
            var scanResult = ScanPlanetsInNearbySectors(Parent, scanSize);
			return scanResult;
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
                        var planetsInThisSector = Parent.Parent.Parent.Galaxy.GetPlanetsInSector(scanSector);
                        var shipsInSector = Parent.Parent.Parent.Parent.ScanShipsInSector(scanSector);

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

        private Interruption CheckInterruptions()
		{
			var roll = SimpleDiceRoller.Roll(DiceType.d20);

			if(roll % 5 == 0)
			{
                roll = SimpleDiceRoller.Roll(DiceType.d100);

                // 20% of the d100 rolls (was 1%), i.e. ~4% per tick instead of 0.2%
                if (roll >= 20 && roll <= 39)
                {
                    State = JourneyState.InBattle;
                    EnemyBattleShip = new SimplePirateShip(Repository.GetRandomShipName(), CurrentSector, null);
                    Parent.Parent.Parent.ActiveSectors.AddShip(CurrentSector, EnemyBattleShip.Id);
                    return new Interruption(InterruptionType.AttackedByPirates, $"Ein Piratenschiff, die {EnemyBattleShip.Name} hat uns erfasst! Wir werden angegriffen!");
                }

                switch (roll)
                {
                    case 9:
                        State = JourneyState.NewPlanetInRange;
                        DiscoveredPlanet = Repository.GenerateRandomPlanet(CurrentSector);
                        Parent.Parent.Parent.DiscoveredPlanets.TryAdd(DiscoveredPlanet.Id, DiscoveredPlanet);
                        Parent.Parent.Parent.Galaxy.TryAdd(DiscoveredPlanet.Id, DiscoveredPlanet);
                        Parent.Parent.Parent.ActiveSectors.AddPlanet(CurrentSector, DiscoveredPlanet.Id);
                        return new Interruption(InterruptionType.DiscoveredNewPlanet, $"Wir haben einen neuen Planeten entdeckt {DiscoveredPlanet.Name} hat uns erfasst! Wir werden angegriffen!");
                    case 3:
                        var randomShipName = "USS Gauntlet";

                        return new Interruption(InterruptionType.DistressSignal, $"Wir haben einen Notruf von der {randomShipName} erhalten.");
                }
            }

		

			return null;
		}
	}
}
