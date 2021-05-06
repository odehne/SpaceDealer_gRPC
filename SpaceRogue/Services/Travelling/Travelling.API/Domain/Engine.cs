using Cope.SpaceRogue.Travelling.API.Domain;
using Cope.SpaceRogue.Travelling.API.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API
{
	public static class Engine
	{
		public static CachedGalaxy Galaxy { get; set; }
		public static List<Journey> Journeys { get; set; }
		public static async Task Init()
		{
			Journeys = new List<Journey>();
			Galaxy = new CachedGalaxy(Factory.Mediator);
			await Galaxy.Load();
		}

		public static void Play()
		{
			do
			{
				Update();
				Thread.Sleep(1000);
			} while (true);
		}

		private static void Update()
		{
			foreach (var journey in Journeys)
			{
				journey.Update();
			}
		}
	}
}
