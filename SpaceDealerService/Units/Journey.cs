using System;

namespace SpaceDealer.Units
{
	public class Journey
	{
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, Coordinates newPosition);

		public Planet Depature { get; set; }
		public Planet Destination { get; set; }
		public Coordinates CurrentSector { get; set; }

		public Journey(Planet departure, Planet destination, Coordinates position)
		{
			Depature = departure;
			Destination = destination;
			CurrentSector = position;
		}

		public double CurrentDistanceToDestination 
		{ 
			get
			{
				return Coordinates.GetDistanceLength(CurrentSector, Destination.Sector);
			}
		} // in parsec 3.26 Light years
		public double CurrentDistanceFromDeparture { get; set; } // in parsec 3.26 Light years
		public double TotalDistance { get; set; }

		public void Update()
		{
			if (!CurrentSector.Equals(Destination.Sector))
			{
				CurrentSector = Coordinates.Move(CurrentSector, Destination.Sector);
				if (CurrentSector.Equals(Destination.Sector))
				{
					Arrived?.Invoke("Arrived at destination.", CurrentSector);
				}
			}
		}
	}
}
