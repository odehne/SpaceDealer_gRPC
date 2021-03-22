using SpaceDealerModels.Repositories;
using SpaceDealerModels.Units;
using SpaceDealerService;
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

		private void FleetCommanders_Arrived(string message, DbCoordinates newPosition, DbShip ship, DbPlayer player)
		{
			Logger.Log($"{player.Name}::{ship.Name} arrived at {ship.Cruise.Destination}", TraceEventType.Information);
		}

		private void FleetCommanders_Interrupted(Enums.InterruptionType interruptionType, string message, DbShip ship, DbPlayer player, DbCoordinates newPosition)
		{
			if (interruptionType == Enums.InterruptionType.DiscoveredNewPlanet)
			{
				Program.Persistor.SaveGalaxy(player.Galaxy);
			}
			Logger.Log($"{player.Name}::{ship.Name} interruped at {newPosition} by {interruptionType}", TraceEventType.Information);
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
			//	Logger.Log(planet.ToString(), TraceEventType.Verbose);
			}
			foreach (var commander in FleetCommanders)
			{
				if(commander!=null)	
					commander.Update();
			//	Logger.Log(commander.ToString(), TraceEventType.Verbose);
			}
		}

		
	}
}
