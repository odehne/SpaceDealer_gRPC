﻿using SpaceDealer.Enums;
using SpaceDealerModels.Repositories;
using System;

namespace SpaceDealerModels.Units
{
	public class Journey
	{
		public Ship Parent { get; set; }
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, Coordinates newPosition);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, Coordinates newPosition);

		public Planet Departure { get; set; }
		public Planet Destination { get; set; }
		public Coordinates CurrentSector { get; set; }
		public JourneyState State { get; set; }
		public PirateShip EnemyBattleShip { get; set; }
		public Planet NewlyDiscoveredPlanet { get; set; }
		
		public Journey(Planet departure, Planet destination, Coordinates position, Ship parent)
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
				return Coordinates.GetDistanceLength(CurrentSector, Destination.Sector);
			}
		} // in parsec 3.26 Light years
		
		public void Update()
		{
			if (State == JourneyState.Travelling)
			{
				if (!CurrentSector.Equals(Destination.Sector))
				{
					State = JourneyState.Travelling;
					CurrentSector = Coordinates.Move(CurrentSector, Destination.Sector);
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