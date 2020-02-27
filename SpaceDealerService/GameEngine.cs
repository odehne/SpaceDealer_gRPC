using SpaceDealer.Units;
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
