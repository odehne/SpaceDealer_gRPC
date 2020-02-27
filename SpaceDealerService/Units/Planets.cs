using System.Collections.Generic;
using System.Linq;

namespace SpaceDealer.Units
{
	public class Planets : List<Planet>
	{
		public Planet GetPlanetByName(string planetName)
		{
			return this.FirstOrDefault(x => x.Name.Equals(planetName, System.StringComparison.InvariantCultureIgnoreCase));
		}
		public Planet GetPlanetInSector(Coordinates coordinates)
		{
			return this.FirstOrDefault(x => x.Sector.X == coordinates.X & x.Sector.Y == coordinates.Y & x.Sector.Z == coordinates.Z);
		}

		public Planets GetAllPlanets(Planet excludeThisPlanet)
		{
			var planets = new Planets();
			planets.AddRange(this.Where(x => !x.Name.Equals(excludeThisPlanet.Name, System.StringComparison.InvariantCultureIgnoreCase)));
			return planets;
		}

		public Planet AddPlanet(Planet newPlanet)
		{
			var p = GetPlanetByName(newPlanet.Name);
			if (p != null)
				return p;
			Add(newPlanet);
			return newPlanet;
		}

		public override string ToString()
		{
			var ret = "";
			foreach (var planet in this)
			{
				ret += planet.ToString() + "\n";
			}
			return ret.TrimEnd('\n');
		}
	}
}
