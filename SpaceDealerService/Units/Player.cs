using SpaceDealer.Enums;
using System.Collections.Generic;

namespace SpaceDealer.Units
{

	public class Player : BaseUnit
	{
		public PlayerTypes PlayerType { get; set; }
		public Planet CurrentPlanet { get; set; }
		public Ships Fleet { get; set; }
		public double Credits { get; set; }
		public Planet HomePlanet { get; set; }

		public Player(string name, List<KeyValuePair<string, string>> properties, Planet homeplanet) : base(name, properties)
		{
			HomePlanet = homeplanet;
			Fleet = new Ships(this);
		}

		public override void Update()
		{
			base.Update();
			foreach(var ship in Fleet)
			{
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
