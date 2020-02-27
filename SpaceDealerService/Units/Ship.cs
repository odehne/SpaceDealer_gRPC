using SpaceDealer.Enums;
using System.Collections.Generic;
using System.Globalization;

namespace SpaceDealer.Units
{
	public class Ship : BaseUnit
	{
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, Coordinates newPosition, Ship ship);

		public Journey Cruise { get; set; }
		public ShipState CurrentState { get; set; }
		public double MaxWeight { get; set; } // in tons
		public ProductsInStock CurrentLoad { get; set; }
		public ShipFeatures Features {get; set;}
		public Ships Parent { get; set; }

		public Ship(string name, List<KeyValuePair<string, string>> properties, Planet homeplanet) : base(name, properties)
		{
			Features = new ShipFeatures();
			CurrentLoad = new ProductsInStock();
			Cruise = new Journey(homeplanet, homeplanet, homeplanet.Sector);
		}

		public void StartCruise(Planet source, Planet destination)
		{
			Cruise = new Journey(source, destination, source.Sector);
			Cruise.Arrived += Cruise_Arrived;
		}

		private void Cruise_Arrived(string message, Coordinates newPosition)
		{
			Arrived?.Invoke(message, newPosition, this);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public Result Load(ProductsInStock productsToLoad)
		{
			if (CurrentState != ShipState.InSpaceDock)
			{
				return new Result(ResultState.Failed, "Das Schiff kann momentan nicht beladen werden.");
			}
			if(productsToLoad.GetTotalWeight() > MaxWeight - CurrentLoad.GetTotalWeight())
			{
				return new Result(ResultState.Failed, $"Das Schiff kann nur maximal {MaxWeight} tons aufnehmen.");
			}
			CurrentLoad.AddRange(productsToLoad);
			return new Result(ResultState.OK, $"Das Schiff wurde beladen. Verfügbarer Speicher {MaxWeight - CurrentLoad.GetTotalWeight()} tons.");
		}

		public override string ToString()
		{
			var distance = Cruise.CurrentDistanceToDestination.ToString("0.##", CultureInfo.InvariantCulture);
			return $"Name: {Name}\tPosition: {Cruise.CurrentSector.ToString()}\t" +
				$"Ziel: {Cruise.Destination.Sector.ToString()}\tEntfernung:{distance} parsec";
		}

		public override void Update()
		{
			base.Update();
			foreach (var feature in Features)
			{
				feature.Update();
			}
			foreach (var load in CurrentLoad)
			{
				load.Update();
			}
			//Evaluate current position
				Cruise.Update();
		}
	}
}
