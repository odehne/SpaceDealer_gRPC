using System.Text.Json.Serialization;
using SpaceDealer.Enums;
using SpaceDealerModels.Attributes;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using Serilog;

namespace SpaceDealerModels.Units
{

	public class DbPlayer : BaseUnit
	{
		[XmlIgnore]
		public Queue UpdateQueue { get; set; }
     
		[JsonPropertyName("playerType")]
		public PlayerTypes PlayerType { get; set; }
		
        [JsonPropertyName("currentPlanet")]
        public DbPlanet CurrentPlanet { get; set; }
		
        [XmlIgnore]
		public Ships Fleet { get; set; }

		[JsonPropertyName("credits")]
		
        public double Credits { get; set; }
		[JsonPropertyName("homePlanet")]
		public DbPlanet HomePlanet { get; set; }

		[JsonPropertyName("discoveredPlanets")]
		public Planets DiscoveredPlanets { get; set; }

		[XmlIgnore]
		[JsonIgnore]
		public Planets Galaxy { get; set; }
		[XmlIgnore]
		[JsonIgnore]
		public Sectors ActiveSectors { get; set; }
        
        [XmlIgnore]
        [JsonIgnore]
        public Game Parent { get; set; }

        public DbPlayer()
		{

		}

        public string GetPrintableMoney(double value)
        {
            CultureInfo culture = new CultureInfo("de-DE");
            string formattedValue = value.ToString("N2", culture);
            return formattedValue;
        }

        public string GetPrintableDouble(double value)
        {
            CultureInfo culture = new CultureInfo("de-DE");
            string formattedValue = Math.Round(value).ToString("N2", culture);
            return formattedValue;
        }

        public DbPlayer(string name, DbPlanet homeplanet, Planets discoveredPlanets, Game game) : base(name)
        {
            Parent = game;
            UpdateQueue = new Queue();
            DiscoveredPlanets = discoveredPlanets;
			HomePlanet = homeplanet;
            Credits = 10000;
            Fleet = new Ships(this);
            DiscoveredPlanets = discoveredPlanets;
            Galaxy = Parent.Galaxy;
			ActiveSectors = Parent.KnownUniverse;
        }

        public void HandleAreaScanned(InterruptionType interruptionType, string message, DbShip ship, DbPlayer player, Planets scannedPlanets, Ships scannedShips)
        {
            // do something with the scanned planets and ships
            // for example, add the scanned planets to the player's discovered planets
            foreach (var planet in scannedPlanets.Values)
            {
                DiscoveredPlanets.TryAdd(planet.Id, planet);
            }
        }
        public void HandleArrival(string message, DbCoordinates newPosition, DbShip ship)
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
            else
            {
                // if the player is real, we can just log the arrival
                Log.Information($"[ARRIVAL] {Name}::{ship.Name} ist auf {ship.CurrentPlanet.Name} angekommen.");
            }
        //    Arrived?.Invoke(message, newPosition, ship, this);
		}

		public void HandleInterruption(InterruptionType interruptionType, string message, DbShip ship, DbCoordinates newPosition)
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

            //Interrupted?.Invoke(interruptionType, message, ship, this, newPosition);
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

          
            foreach (var ship in Fleet.Values)
            {


                if (ship.State == ShipState.Trading)
                {
                   
                    var industry = ship.CurrentPlanet.Industry;
					if(ship.CargoBay != null && ship.CargoBay.Any())
					{
                        // sell our stuff
						for (int i = 0; i < ship.CargoBay.Count; i++)
                        {
                            var load = ship.CargoBay[i];
                            if (load != null && load.Quantity > 0)
                            {
                                if (industry.ProductsNeeded != null && industry.ProductsNeeded.Any())
                                {                                     // sell it
                                    var product = industry.ProductsNeeded.GetByName(load.Product.Name);
                                    if (product != null)
                                    {
                                        Credits += (product.GetTotalPrice() * load.Quantity);
                                        ship.CargoBay.Remove(load);
                                        Log.Information($"[TRADE] {Name}::{ship.Name} hat {GetPrintableDouble(load.Quantity)}t von {load.Product.Name} für " + GetPrintableMoney(product.GetTotalPrice() * load.Quantity) + " credits an " + ship.CurrentPlanet.Name + " verkauft. Kontostand: " + GetPrintableMoney(Credits));
                                    }
                                    else
                                    {
                                        Credits += (load.GetTotalPrice() * load.Quantity);
                                        ship.CargoBay.Remove(load);
                                        Log.Information($"[TRADE] {Name}::{ship.Name} hat {GetPrintableDouble(load.Quantity)}t von {load.Product.Name} für " + GetPrintableMoney(load.GetTotalPrice() * load.Quantity ) + " credits an " + ship.CurrentPlanet.Name + " verkauft. Kontostand: " + GetPrintableMoney(Credits));
                                    }
                                }
                                else
                                {
                                    Credits += (load.GetTotalPrice() * load.Quantity);
                                    ship.CargoBay.Remove(load);
                                    Log.Information($"[TRADE] {Name}::{ship.Name} hat {GetPrintableDouble(load.Quantity)}t von {load.Product.Name} für " + GetPrintableMoney(load.GetTotalPrice() * load.Quantity) + " credits an " + ship.CurrentPlanet.Name + " verkauft. Kontostand: " + GetPrintableMoney(Credits));
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
                            var quantity = (int)(ship.CargoSize - ship.CargoBay.TotalQuantity);

							if(quantity > product.Amount)
                                quantity = (int)product.Amount;

                            // how much can we afford?
                            var cost = product.GetTotalPrice() * quantity;

                            if (cost > Credits)
                            {
                                Log.Warning($"[TRADE] {Name}::{ship.Name} kann sich nicht genug von {product.Name} leisten. Verfügbares Geld: {GetPrintableMoney(Credits)}, benötigter Betrag: {GetPrintableMoney(cost)}");
                                quantity = (int)Math.Floor(Credits / product.GetTotalPrice());
                            }
                                

                            // load it
                            if (quantity > 0)
                            {
                                ship.CargoBay.AddCargo(ship, product, quantity);
                                Credits -= (product.GetTotalPrice() * quantity);
                                Log.Information($"[TRADE] {Name}::{ship.Name} hat {GetPrintableDouble(quantity)}t von {product.Name} für " + GetPrintableMoney(product.GetTotalPrice() * quantity) + " credits auf " + ship.CurrentPlanet.Name + " gekauft. Kontostand: " + GetPrintableMoney(Credits));
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
            return DiscoveredPlanets.Values.ElementAt(i);
        }

        public override void Update()
		{
			base.Update();
			foreach(var ship in Fleet.Values)
			{
				if(ship.State != ShipState.Sunken)
                    ship.Update();

                if (ship.State == ShipState.InJourney)
                    ship.ScanSpace();
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
