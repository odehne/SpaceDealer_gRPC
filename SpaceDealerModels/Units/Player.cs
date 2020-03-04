using Newtonsoft.Json;
using SpaceDealer.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpaceDealerModels.Units
{

	public class Player : BaseUnit
	{
		[Newtonsoft.Json.JsonIgnore]
		public Queue UpdateQueue { get; set; }

		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, Coordinates newPosition, Ship ship, Player player);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, Ship ship, Player player, Coordinates newPosition);

		[JsonProperty("playerType")]
		public PlayerTypes PlayerType { get; set; }
		[JsonProperty("currentPlanet")]
		public Planet CurrentPlanet { get; set; }
		[JsonProperty("fleet")]
		public Ships Fleet { get; set; }
		[JsonProperty("credits")]
		public double Credits { get; set; }
		[JsonProperty("homePlanet")]
		public Planet HomePlanet { get; set; }
		[JsonProperty("galaxy")]
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
