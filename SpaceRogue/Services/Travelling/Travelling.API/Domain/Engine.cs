using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Travelling.API.Domain;
using Cope.SpaceRogue.Travelling.API.Models;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API
{
	public class Engine
	{

		private static ILogger _logger = Log.ForContext<Engine>();
		public static GalaxyModel Galaxy { get; set; }
		public static Journeys Journeys { get; set; }
		public static async Task Init()
		{
			Journeys = new Journeys();
			Galaxy = new GalaxyModel(Factory.Mediator);
			await Galaxy.Load();
		}

		public static Journey AddJourney(ShipModel ship, Position sourcePosition, Position destinationPosition, DestinationTypes destinationType, int speed = 1)
		{
			var journey = new Journey(Guid.NewGuid(), ship, sourcePosition, destinationPosition, destinationType, speed);
			journey.Arrived += Journey_Arrived;
			journey.Interrupted += Journey_Interrupted;

			Journeys.AddJourney(journey);
			return journey;
		}

		private static void Journey_Interrupted(Guid journeyId, InterruptionTypes interruptionType, string message, Position newPosition, Guid shipId)
		{
			_logger.Information($"{newPosition}: Interrupted by [{interruptionType}].");
			//Journeys.RemoveJourney(journeyId);
		}

		private static void Journey_Arrived(Guid journeyId, string message, Position newPosition, Guid shipId)
		{
			_logger.Information($"Destination reached, removing journey [{journeyId}].");
			Galaxy.PersistShipsPosition(Galaxy.GetShip(shipId));
			Journeys.RemoveJourney(journeyId);
		}


		public static void Play()
		{
			var i = 1;
			do
			{
				Update();
				Thread.Sleep(1000);
				if (i == 5)
					i = 1;
				else
					i++;
			} while (true);
		}

		private static void Update()
		{
			foreach (var journey in Journeys)
			{
				journey.Value.Update();
			}
		}
	}
}
