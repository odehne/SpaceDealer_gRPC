using SpaceDealer.Enums;
using System.Collections;
using System.Collections.Generic;

namespace SpaceDealerModels.Units
{

	public class Player : BaseUnit
	{
		public Queue UpdateQueue { get; set; }

		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, Coordinates newPosition, Ship ship, Player player);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, Ship ship, Player player, Coordinates newPosition);

		public PlayerTypes PlayerType { get; set; }
		public Planet CurrentPlanet { get; set; }
		public Ships Fleet { get; set; }
		public double Credits { get; set; }
		public Planet HomePlanet { get; set; }
		public Planets Galaxy { get; set; }


		public Player(string name, Planet homeplanet, Planets planets) : base(name)
		{
			UpdateQueue = new Queue();
			Galaxy = planets;
			HomePlanet = homeplanet;
			Fleet = new Ships(this);
			Fleet.Interrupted += Fleet_Interrupted;
			Fleet.Arrived += Fleet_Arrived;
		}

		private void Fleet_Arrived(string message, Coordinates newPosition, Ship ship)
		{
			UpdateQueue.Enqueue(new UpdateInfo(ship, UpdateStates.ArrivedOnTarget));
			Arrived?.Invoke(message, newPosition, ship, this);
		}

		private void Fleet_Interrupted(InterruptionType interruptionType, string message, Ship ship, Coordinates newPosition)
		{
			switch (interruptionType)
			{
				case InterruptionType.AttackedByPirates:
					UpdateQueue.Enqueue(new UpdateInfo(ship, UpdateStates.UnderAttack));
					break;
				case InterruptionType.DiscoveredNewPlanet:
					UpdateQueue.Enqueue(new UpdateInfo(ship, UpdateStates.NewPlanetDiscovered));
					break;
				case InterruptionType.DistressSignal:
					UpdateQueue.Enqueue(new UpdateInfo(ship, UpdateStates.OnRescueMission));
					break;
			}
			Interrupted?.Invoke(interruptionType, message, ship, this, newPosition);
		}

		public override void Update()
		{
			base.Update();
			foreach(var ship in Fleet)
			{
				ship.Update();
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return $"Name: {Name}\tCredits: {Credits}\tSchiffe: {Fleet.ToString()}";
		}
	}
}
