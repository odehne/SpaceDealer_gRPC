using SpaceDealerModels.Units;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace SpaceDealer
{
	public class GameEngine
	{
		public Players FleetCommanders { get; set; }
		public Planets Galaxy { get; set; }
		public ILogger Logger { get; set; }

		public GameEngine(ILogger logger, Planets galaxy, Players fleetCommanders)
		{
			Logger = logger;
			Galaxy = galaxy;
			FleetCommanders = fleetCommanders;
			FleetCommanders.Interrupted += FleetCommanders_Interrupted;
			FleetCommanders.Arrived += FleetCommanders_Arrived;
		}
		
		private void FleetCommanders_Arrived(string message, Coordinates newPosition, Ship ship, Player player)
		{
			Logger.Log($"{player.Name}::{ship.Name} arrived at {ship.Cruise.Destination}", TraceEventType.Information);
		}

		private void FleetCommanders_Interrupted(Enums.InterruptionType interruptionType, string message, Ship ship, Player player, Coordinates newPosition)
		{
			Logger.Log($"{player.Name}::{ship.Name} interruped at {newPosition.ToString()} by {interruptionType.ToString()}", TraceEventType.Information);
		}

		public void Play()
		{
			do
			{
				Update();
				Thread.Sleep(1000);
			} while (true);
		}

		public void Update()
		{
			foreach (var planet in Galaxy)
			{
				planet.Update();
				Logger.Log(planet.ToString(), TraceEventType.Verbose);
			}
			foreach (var commander in FleetCommanders)
			{
				commander.Update();
				Logger.Log(commander.ToString(), TraceEventType.Verbose);
			}
		}

		
	}
}
