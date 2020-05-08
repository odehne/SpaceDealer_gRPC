using Newtonsoft.Json;
using SpaceDealer.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpaceDealerModels.Units
{

	public class DbPlayer : BaseUnit
	{
		[Newtonsoft.Json.JsonIgnore]
		public Queue UpdateQueue { get; set; }

		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, DbCoordinates newPosition, DbShip ship, DbPlayer player);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, DbShip ship, DbPlayer player, DbCoordinates newPosition);

		[JsonProperty("playerType")]
		public PlayerTypes PlayerType { get; set; }
		[JsonProperty("currentPlanet")]
		public DbPlanet CurrentPlanet { get; set; }
		[JsonProperty("fleet")]
		public Ships Fleet { get; set; }
		[JsonProperty("credits")]
		public double Credits { get; set; }
		[JsonProperty("homePlanet")]
		public DbPlanet HomePlanet { get; set; }
		[JsonProperty("galaxy")]
		public Planets Galaxy { get; set; }
		
		public DbPlayer()
		{
		}

		public DbPlayer(string name, DbPlanet homeplanet, Planets planets) : base(name)
		{
			UpdateQueue = new Queue();
			Galaxy = planets;
			HomePlanet = homeplanet;
			Credits = 10000;
			Fleet = new Ships(this);
			Fleet.Interrupted += Fleet_Interrupted;
			Fleet.Arrived += Fleet_Arrived;
		}

		private void Fleet_Arrived(string message, DbCoordinates newPosition, DbShip ship)
		{
			UpdateQueue.Enqueue(new UpdateInfo(ship, UpdateStates.ArrivedOnTarget));
			Arrived?.Invoke(message, newPosition, ship, this);
		}

		private void Fleet_Interrupted(InterruptionType interruptionType, string message, DbShip ship, DbCoordinates newPosition)
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
