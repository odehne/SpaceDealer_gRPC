using Cope.SpaceRogue.InfraStructure;
using Infrastructure.Domain;
using Serilog;
using System;

namespace Cope.SpaceRogue.Travelling.API.Domain
{

	public enum InterruptionType
	{
		AttackedByPirates = 0,
		OtherShipWantsToTrade = 1,
		DistressSignal = 2,
		DiscoveredNewPlanet = 3
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
		public delegate void ArrivedAtDestination(string message, Position newPosition);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, Position newPosition);


		public Guid Id { get; }
		public Guid ShipId { get; }
		public Position Source { get; set; }
		public Position Destination { get; set; }
		public Position CurrentPosition { get; set; }
		public int Speed { get; set; }
		public JourneyStates State { get; set; }
		public object NewlyDiscoveredPlanet { get; private set; }

		public Journey(Guid id, Guid shipId, Position source, Position destination, Position currentPosition, int speed = 1)
		{
			Id = id;
			ShipId = shipId;
			Source = source;
			Destination = destination;
			CurrentPosition = currentPosition;
			Speed = speed;

			if (currentPosition == destination)
				Speed = 0;
		}

		public void Update()
		{
			Log.Information($"Updating journey of {ShipId} at {CurrentPosition}.");
			if (Speed > 0)
			{
				CurrentPosition = Position.Move(CurrentPosition, Destination, Speed);
			}

			if (CurrentPosition.Equals(Destination))
			{
				CurrentPosition = Destination;
				Arrived?.Invoke("Arrived at destination.", CurrentPosition);
			}
			else
			{
				var interruption = CheckInterruptions();
				if (interruption != null)
				{
					Interrupted?.Invoke(interruption.Type, interruption.Message, CurrentPosition);
				}
			}
		}

		private Interruption CheckInterruptions()
		{
			var roll = SimpleDiceRoller.Roll();
			switch (roll)
			{
				case 6:
					State = JourneyStates.UnderAttack;
					//EnemyBattleShip = new SimplePirateShip(Repository.GetRandomShipName(), CurrentSector, null);
					return new Interruption(InterruptionType.AttackedByPirates, $"Ein Piratenschiff, die [NAME] hat uns erfasst! Wir werden angegriffen!");
				case 9:
					State = JourneyStates.FoundNewPlanet;
					return new Interruption(InterruptionType.DiscoveredNewPlanet, $"Wir haben einen neuen Planeten entdeckt [NAME] hat uns erfasst! Wir werden angegriffen!");
				case 3:
					var randomShipName = "USS Gauntlet";
					return new Interruption(InterruptionType.DistressSignal, $"Wir haben einen Notruf von der {randomShipName} erhalten.");
			}

			return null;
		}
	}
}