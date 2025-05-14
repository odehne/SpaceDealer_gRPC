using Newtonsoft.Json;
using SpaceDealer.Enums;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;

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
		[JsonProperty("discoveredPlanets")]
		public Planets DiscoveredPlanets { get; set; }
		[Newtonsoft.Json.JsonIgnore]
		public Planets Galaxy { get; set; }
		[Newtonsoft.Json.JsonIgnore]
		public Sectors ActiveSectors { get; set; }

		public DbPlayer()
		{

		}

        public string GetPrintableMoney(double value)
        {

            CultureInfo culture = new CultureInfo("de-DE");
            string formattedValue = value.ToString("N2", culture);
            return formattedValue;

        }

        public DbPlayer(string name, DbPlanet homeplanet, Planets discoveredPlanets, Planets galaxy, Sectors activeSectors) : base(name)
        {
            UpdateQueue = new Queue();
            DiscoveredPlanets = discoveredPlanets;
			ActiveSectors = activeSectors;
			HomePlanet = homeplanet;
            Credits = 10000;
            Fleet = new Ships(this);
            Fleet.Interrupted += Fleet_Interrupted;
            Fleet.Arrived += Fleet_Arrived;
            DiscoveredPlanets = discoveredPlanets;
            Galaxy = galaxy;
			ActiveSectors = activeSectors;
        }

        private void Fleet_Arrived(string message, DbCoordinates newPosition, DbShip ship)
		{
			UpdateQueue.Enqueue(new UpdateInfo(ship, UpdateStates.ArrivedOnTarget));

			if(PlayerType == PlayerTypes.NPC)
            {
                ship.State = ShipState.Trading;
				SimulateTrading();
                // get new destination
				ship.State = ShipState.InJourney;
                ship.StartCruise(ship.CurrentPlanet, GetRandomDiscoveredPlanet());
            }

            Arrived?.Invoke(message, newPosition, ship, this);
		}

		private void Fleet_Interrupted(InterruptionType interruptionType, string message, DbShip ship, DbCoordinates newPosition)
		{
			switch (interruptionType)
			{
				case InterruptionType.AttackedByPirates:
					UpdateQueue.Enqueue(new UpdateInfo(ship, UpdateStates.UnderAttack));
                    if (PlayerType == PlayerTypes.NPC)
                    {
                        ship.State = ShipState.Attacked;
                        SimulateBattle(ship);
                        // get new destination
                        ship.State = ShipState.InJourney;
                        ship.StartCruise(ship.CurrentPlanet, GetRandomDiscoveredPlanet());
                    }
                    else
                        UpdateQueue.Enqueue(new UpdateInfo(ship, UpdateStates.NewPlanetDiscovered));
                    break;
				case InterruptionType.DiscoveredNewPlanet:
                    if (PlayerType == PlayerTypes.NPC)
                        ship.State = ShipState.Idle;
					else
                        UpdateQueue.Enqueue(new UpdateInfo(ship, UpdateStates.NewPlanetDiscovered));
                    break;
				case InterruptionType.DistressSignal:
					UpdateQueue.Enqueue(new UpdateInfo(ship, UpdateStates.OnRescueMission));
					break;
			}

            Interrupted?.Invoke(interruptionType, message, ship, this, newPosition);
		}

        private void SimulateBattle(DbShip ship)
        {
            var attackResult = ship.Attack();
            
            if(attackResult.Defeaded)
            {
                ship.Parent.Parent.Credits += attackResult.Treasure;
                ship.State = ShipState.Idle;
                ship.Cruise.ContinueTravel();
                Console.WriteLine("[BATTLE] " + ship.Name + " attacked and defeated the enemy ship and got " + attackResult.Treasure + " credits.");
                return;
            }
            else
            {
                Console.WriteLine("[BATTLE] " + attackResult.Message);
            }

            var defenceResult = ship.Defend();
            if (defenceResult.Defeaded)
            {
                ship.Parent.Parent.Credits -= attackResult.Treasure;
                ship.State = ShipState.Sunken;
                Console.WriteLine("[BATTLE] " + ship.Name + " was defeated. Pirates escaped with " + attackResult.Treasure + " credits.");
                return;
            }
            else
            {
                Console.WriteLine("[BATTLE] " + attackResult.Message);
            }

            ship.State = ShipState.Idle;
            ship.Cruise.ContinueTravel();
        }


        private void SimulateTrading()
		{

          
            foreach (var ship in Fleet)
            {


                if (ship.State == ShipState.Trading)
                {
                   
                    var industry = ship.CurrentPlanet.Industry;
					if(ship.CurrentLoad != null && ship.CurrentLoad.Any())
					{
                        // sell our stuff
						for (int i = 0; i < ship.CurrentLoad.Count; i++)
                        {
                            var load = ship.CurrentLoad[i];
                            if (load != null && load.Amount > 0)
                            {
                                if (industry.ProductsNeeded != null && industry.ProductsNeeded.Any())
                                {                                     // sell it
                                    var product = industry.ProductsNeeded.GetProductByName(load.Name);
                                    if (product != null)
                                    {
                                        Credits += (product.GetTotalPrice() * load.Amount);
                                        ship.CurrentLoad.Remove(load);
                                        Console.WriteLine($"[TRADE] {Name}::{ship.Name} hat {load.Amount}t von {load.Name} für " + GetPrintableMoney(product.GetTotalPrice() * load.Amount) + " credits an " + ship.CurrentPlanet.Name + " verkauft. Kontostand: " + GetPrintableMoney(Credits));
                                    }
                                    else
                                    {
                                        Credits += (load.GetTotalPrice() * load.Amount);
                                        ship.CurrentLoad.Remove(load);
                                        Console.WriteLine($"[TRADE] {Name}::{ship.Name} hat {load.Amount}t von {load.Name} für " + GetPrintableMoney(load.GetTotalPrice() * load.Amount) + " credits an " + ship.CurrentPlanet.Name + " verkauft. Kontostand: " + GetPrintableMoney(Credits));
                                    }
                                }
                                else
                                {
                                    Credits += (load.GetTotalPrice() * load.Amount);
                                    ship.CurrentLoad.Remove(load);
                                    Console.WriteLine($"[TRADE] {Name}::{ship.Name} hat {load.Amount}t von {load.Name} für " + GetPrintableMoney(load.GetTotalPrice() * load.Amount) + " credits an " + ship.CurrentPlanet.Name + " verkauft. Kontostand: " + GetPrintableMoney(Credits));
                                }
                            }
                        }
                    }
					else
					{
                        // buy some stuff
                        var product = industry.GeneratedProducts.FirstOrDefault(x => x.GetTotalPrice() < Credits);
                        if (product != null)
                        {
                            // how much can we load?
                            var amount = (int)(ship.CargoSize - ship.CurrentLoad.GetTotalWeight());

							if(amount > product.Amount)
                                amount = (int)product.Amount;

                            // how much can we afford?
                            var cost = product.GetTotalPrice() * amount;

                            if (cost > Credits)
                                amount = (int)Math.Floor(Credits / product.GetTotalPrice());

                            // load it
                            if (amount > 0)
                            {
                                ship.CurrentLoad.Add(product);
								ship.CurrentLoad.Last().Amount = amount;
                                Credits -= (product.GetTotalPrice() * amount);
                                Console.WriteLine($"[TRADE] {Name}::{ship.Name} hat {amount}t von {product.Name} für " + GetPrintableMoney(product.GetTotalPrice() * amount) + " credits auf " + ship.CurrentPlanet.Name + " gekauft. Kontostand: " + GetPrintableMoney(Credits));
                            }
                        }
                    }
                }
            }
        }

        public DbPlanet GetRandomDiscoveredPlanet()
        {
			if(DiscoveredPlanets.Count == 0)
                return Galaxy.GetRandomPlanet();

            Random random = new Random();
            var i = random.Next(0, DiscoveredPlanets.Count - 1);
            return DiscoveredPlanets[i];
        }

        public override void Update()
		{
			base.Update();
			foreach(var ship in Fleet)
			{
				if(ship.State != ShipState.Sunken)
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
