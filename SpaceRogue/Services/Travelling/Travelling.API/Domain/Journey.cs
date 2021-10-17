using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Domain;
using Cope.SpaceRogue.Travelling.API.Models;
using Infrastructure.Domain;
using Serilog;
using System;

namespace Cope.SpaceRogue.Travelling.API.Domain
{
	public enum DestinationTypes
	{
		Planet,
		SpaceShip,
		SpaceStation,
		Astroid
	}

	public enum InterruptionTypes
	{
		AttackedByPirates = 1,
		AnotherShipWantsToTrade = 2,
		DistressCall = 3,
		DiscoveredNewPlanet = 4,
		DiscoveredWreck = 5,
		DiscoveredProbe = 6
	}

	public enum JourneyStates
	{
		Idle,
		JourneyStarted,
		ArrivedAtPosition,
		ArrivedAtPlanet,
		ArrivedAtShipyard,
		BuyingAndSelling,
		Fighting,
		RecievedDistressCall,
		FoundNewPlanet,
		RecievedInformationOffering,
		JourneyFinished,
		UnderAttack
	}

	public class Journey
	{
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(Guid journeyId, string message, Position newPosition, Guid shipId);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(Guid journeyId, InterruptionTypes interruptionType, string message, Position newPosition, Guid shipId);

		public Guid Id { get; }
		public ShipModel Ship { get; }
		public Position Source { get; set; }
		public Position Destination { get; set; }
		public Position CurrentPosition { get; set; }
		public int Speed { get; set; }
		public JourneyStates State { get; set; }
		public PlanetModel NewlyDiscoveredPlanet { get; private set; }
		public DestinationTypes DestinationType { get; set; }

		public Journey(Guid id, ShipModel ship, Position source, Position destination, DestinationTypes destinationType, int speed = 1)
		{
			Id = id;
			Ship = ship;
			Source = source;
			Destination = destination;
			CurrentPosition = ship.CurrentSector;
			Speed = speed;
			DestinationType = destinationType;

			if (CurrentPosition == destination)
			{
				Arrived?.Invoke(Id, "Arrived at destination.", CurrentPosition, Id);
				Speed = 0;
			}
		}

		public void Update()
		{
			Log.Verbose($"Updating journey of {Ship.ShipId} at {CurrentPosition}.");
			if (Speed > 0)
			{
				CurrentPosition = Position.Move(CurrentPosition, Destination, Speed);
				Engine.Galaxy.UpdateShipPosition(Ship.ShipId, CurrentPosition);
			}

			if (CurrentPosition.Equals(Destination))
			{
				CurrentPosition = Destination;
				Arrived?.Invoke(Id, "Arrived at destination.", CurrentPosition, Ship.ShipId);
			}
			else
			{
				var interruption = CheckInterruptions();
				if (interruption != null)
				{
					Interrupted?.Invoke(Id, interruption.Type, interruption.Message, CurrentPosition, Ship.ShipId);
				}
			}
		}

		private InterruptionBase CheckInterruptions()
		{
			var roll = SimpleDiceRoller.Roll();
			switch (roll)
			{
				case 6:
				case 9:
				case 3:
					return GetRandomDiscovery();
			}

			return null;
		}

		private InterruptionBase GetRandomDiscovery()
		{
			var randomDiscovery = SimpleDiceRoller.Roll(DiceType.d6);
			var randomShipName = "USS Gauntlet";
			var interruptionType = (InterruptionTypes)randomDiscovery;

			switch (interruptionType)
			{
				case InterruptionTypes.AttackedByPirates:
					return new AttackedByPiratesInterruption($"Wir werden vom Piratenshiff {randomShipName} angegriffen.");
				case InterruptionTypes.DiscoveredNewPlanet:
					return new DiscoveredNewPlanetInterruption( $"Wir haben einen Notruf von der {randomShipName} erhalten.");
				case InterruptionTypes.DiscoveredProbe:
					return new DiscoveredProbeInterruption($"Wir haben einen Notruf von der {randomShipName} erhalten.");
				case InterruptionTypes.DiscoveredWreck:
					return new DiscoveredWreckInterruption( $"Wir haben einen Notruf von der {randomShipName} erhalten.");
				case InterruptionTypes.DistressCall:
					return new DistresscallInterruption( $"Wir haben einen Notruf von der {randomShipName} erhalten.");
				case InterruptionTypes.AnotherShipWantsToTrade:
					return new AnotherShipWantsToTradeInterruption($"Wir haben einen Notruf von der {randomShipName} erhalten.");
			}

			return null;
		}
	}
}