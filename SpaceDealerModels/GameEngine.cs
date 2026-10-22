using Serilog;
using SpaceDealerModels.Repositories;
using SpaceDealerModels.Units;
using System;
using System.Diagnostics;
using System.Threading;

namespace SpaceDealer
{

   

    public class GameEngine
	{

        public Game Game { get; set; }

        public ILogger Logger { get; set; }

		public GameEngine(ILogger logger, Planets galaxy, Players fleetCommanders)
		{

            Game = new Game
            {
                Galaxy = galaxy,
                FleetCommanders = fleetCommanders,
                KnownUniverse = new Sectors()
            };


            Logger = logger;
        
        }

		public void Play()
		{
			do
			{
                Game.Update();
                Thread.Sleep(1000);
			} while (true);
		}

	}
}
