using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Travelling.API.Domain;
using Cope.SpaceRogue.Travelling.API.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API
{
	public static class Engine
	{
		public static GalaxyModel Galaxy { get; set; }
		public static Journeys Journeys { get; set; }
		public static async Task Init()
		{
			Journeys = new Journeys();
			Galaxy = new GalaxyModel(Factory.Mediator);
			await Galaxy.Load();
		}

		public static Journey AddJourney(Guid shipId, Position sourcePosition, Position currentPosition, Position destinationPosition, DestinationTypes destinationType, int speed = 1)
		{
			var journey = new Journey(Guid.NewGuid(), shipId, sourcePosition, destinationPosition, currentPosition, destinationType, speed);
			journey.Arrived += Journey_Arrived;
			journey.Interrupted += Journey_Interrupted;

			Journeys.AddJourney(journey);
			return journey;
		}

		private static void Journey_Interrupted(Guid journeyId, InterruptionTypes interruptionType, string message, Position newPosition, Guid shipId)
		{
			Journeys.RemoveJourney(journeyId);
		}

		private static void Journey_Arrived(Guid journeyId, string message, Position newPosition, Guid shipId)
		{
			throw new NotImplementedException();
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
