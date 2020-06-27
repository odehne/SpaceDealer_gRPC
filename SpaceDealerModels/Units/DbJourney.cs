using Newtonsoft.Json;
using SpaceDealer.Enums;
using SpaceDealerModels.Repositories;
using System;

namespace SpaceDealerModels.Units
{
	public class DbJourney
	{
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, DbCoordinates newPosition);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, DbCoordinates newPosition);

		[JsonIgnore]
		public DbShip Parent { get; set; }
		[JsonProperty("departure")]
		public DbPlanet Departure { get; set; }
		[JsonProperty("detination")]
		public DbPlanet Destination { get; set; }
		[JsonProperty("currentSector")]
		public DbCoordinates CurrentSector { get; set; }
		[JsonProperty("state")]
		JourneyState State { get; set; }
		[JsonProperty("enemyBattleShip")]
		public PirateShip EnemyBattleShip { get; set; }
		[JsonProperty("newlyDisoveredPlanet")]
		public DbPlanet NewlyDiscoveredPlanet { get; set; }
		
		public DbJourney()
		{

		}

		public DbJourney(DbPlanet departure, DbPlanet destination, DbCoordinates position, DbShip parent)
		{
			Parent = parent;
			Departure = departure;
			Destination = destination;
			CurrentSector = position;
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
			if (State == JourneyState.Travelling)
			{
				if (!CurrentSector.Equals(Destination.Sector))
				{
					State = JourneyState.Travelling;
					CurrentSector = DbCoordinates.Move(CurrentSector, Destination.Sector);
				}
				if (CurrentSector.Equals(Destination.Sector))
				{
					CurrentSector = Destination.Sector;
					Departure = Destination;
					State = JourneyState.Arrived;
					Arrived?.Invoke("Arrived at destination.", CurrentSector);
				}
				else
				{
					var interruption = CheckInterruptions();
					if (interruption != null)
					{
						Interrupted?.Invoke(interruption.Type, interruption.Message, CurrentSector);
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

		private Interruption CheckInterruptions()
		{
			var roll = SimpleDiceRoller.Roll();
			switch(roll)
			{
				case 6:
					State = JourneyState.InBattle;
					EnemyBattleShip = new SimplePirateShip(Repository.GetRandomShipName(), CurrentSector, null);
					return new Interruption(InterruptionType.AttackedByPirates, $"Ein Piratenschiff, die {EnemyBattleShip.Name} hat uns erfasst! Wir werden angegriffen!");
				case 9:
					State = JourneyState.NewPlanetInRange;
					NewlyDiscoveredPlanet = Repository.GetRandomPlanet(CurrentSector);
					Parent.Parent.Parent.Galaxy.Add(NewlyDiscoveredPlanet);
					return new Interruption(InterruptionType.DiscoveredNewPlanet, $"Wir haben einen neuen Planeten entdeckt {NewlyDiscoveredPlanet.Name} hat uns erfasst! Wir werden angegriffen!");
				case 3:
					var randomShipName = "USS Gauntlet";
					return new Interruption(InterruptionType.DistressSignal, $"Wir haben einen Notruf von der {randomShipName} erhalten.");
			}

			return null;
		}
	}
}
