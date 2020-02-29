using SpaceDealer.Enums;
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

		public Planet Depature { get; set; }
		public Planet Destination { get; set; }
		public Coordinates CurrentSector { get; set; }
		public JourneyState State { get; set; }
		
		public Journey(Planet departure, Planet destination, Coordinates position, Ship parent)
		{
			Parent = parent;
			Depature = departure;
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
			if (!CurrentSector.Equals(Destination.Sector))
			{
				State = JourneyState.Travelling;
				CurrentSector = Coordinates.Move(CurrentSector, Destination.Sector);
			}
			if (CurrentSector.Equals(Destination.Sector))
			{
				State = JourneyState.Arrived;
				Arrived?.Invoke("Arrived at destination.", CurrentSector);
			}
			else
			{
				var interruption = CheckInterruptions();
			}
			
		}

		private Interruption CheckInterruptions()
		{
			var randomShipName = "USS Gauntlet";
			return new Interruption(InterruptionType.DistressSignal, $"Wir haben einen Notruf von der {randomShipName} erhalten.");
		}
	}
}
