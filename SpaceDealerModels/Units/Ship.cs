using SpaceDealer.Enums;
using System.Collections.Generic;
using System.Globalization;

namespace SpaceDealerModels.Units
{
	public class Ship : BaseUnit
	{
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, Coordinates newPosition, Ship ship);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, Coordinates newPosition);

		public Journey Cruise { get; set; }
		public ShipState CurrentState { get; set; }
		public double CargoSize { get; set; } // in tons
		public ProductsInStock CurrentLoad { get; set; }
		public ShipFeatures Features {get; set;}
		public Ships Parent { get; set; }
		public ShipState State { get; set; }

		public Ship(string name, List<KeyValuePair<string, string>> properties, Planet homeplanet, ShipFeatures featureSet) : base(name, properties)
		{
			Features = featureSet;
			CurrentLoad = new ProductsInStock();
			Cruise = new Journey(homeplanet, homeplanet, homeplanet.Sector, this);
		}

		public void StartCruise(Planet source, Planet destination)
		{
			Cruise = new Journey(source, destination, source.Sector, this);
			Cruise.Arrived += Cruise_Arrived;
			Cruise.Interrupted += Cruise_Interrupted;
		}

		private void Cruise_Interrupted(InterruptionType interruptionType, string message, Coordinates newPosition)
		{
			Interrupted?.Invoke(interruptionType, message, newPosition);
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
			if (CurrentState != ShipState.Idle)
			{
				return new Result(ResultState.Failed, "Das Schiff kann momentan nicht beladen werden.");
			}
			if(productsToLoad.GetTotalWeight() > CargoSize - CurrentLoad.GetTotalWeight())
			{
				return new Result(ResultState.Failed, $"Das Schiff kann nur maximal {CargoSize} tons aufnehmen.");
			}
			CurrentLoad.AddRange(productsToLoad);
			return new Result(ResultState.OK, $"Das Schiff wurde beladen. Verfügbarer Speicher {CargoSize - CurrentLoad.GetTotalWeight()} tons.");
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

		public int Battle()
		{
			for (int i = 0; i < 5; i++)
			{
				var shipDefenceRoll = SimpleDiceRoller.Roll(DiceType.d20, 1);
				var pirateAttackRoll = SimpleDiceRoller.Roll(DiceType.d6) + SimpleDiceRoller.Roll(DiceType.d6);
				//TheLogger.Log($"Runde {i}: Angriff: {pirateAttackRoll} vs. Verteidigung: {shipDefenceRoll}", TraceEventType.Information);
			}
			return -1;
		}
	}
}
