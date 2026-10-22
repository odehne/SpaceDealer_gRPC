using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
    public class Planets : ConcurrentDictionary<string, DbPlanet>
	{

		public Planets GetFirstFivePlanets()
		{
			var ret = new Planets();
			for (int i = 0; i < 4; i++)
			{
				ret.AddPlanet(this.Values.ElementAt(i));
			}
			return ret;
		}

		public DbPlanet GetPlanetByName(string planetName)
		{
			return this.Values.FirstOrDefault(x => x.Name.Equals(planetName, StringComparison.InvariantCultureIgnoreCase));
		}
		public DbPlanet GetPlanetInSector(DbCoordinates coordinates)
		{
			return this.Values.FirstOrDefault(x => x.Sector.X == coordinates.X & x.Sector.Y == coordinates.Y & x.Sector.Z == coordinates.Z);
		}

        public Planets GetPlanetsInSector(DbCoordinates coordinates)
        {
            var planets = new Planets();

            foreach (var planet in Values.Where(x => x.Sector.X == coordinates.X && x.Sector.Y == coordinates.Y && x.Sector.Z == coordinates.Z))
				planets.TryAdd(planet.Id, planet);

            return planets;
        }

        public DbPlanet GetPlanetById(string id)
        {
            return this.Values.FirstOrDefault(x => x.Id == id);
        }

        public Planets GetAllPlanets(DbPlanet excludeThisPlanet)
		{
			var planets = new Planets();

            foreach (var planet in Values.Where(x => !x.Name.Equals(excludeThisPlanet.Name, StringComparison.InvariantCultureIgnoreCase)))
                planets.TryAdd(planet.Id, planet);

            return planets;
		}

		public DbPlanet AddPlanet(DbPlanet newPlanet)
		{
			var p = GetPlanetByName(newPlanet.Name);
			if (p != null)
				return p;
			this.TryAdd(newPlanet.Id, newPlanet);
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

        public Planets GetRandomPlanets(int howMany)
        {
            var randomPlanets = new Planets();
			for (int i = 0; i < howMany; i++)
			{
				var rp = GetRandomPlanet();
				if (!randomPlanets.ContainsKey(rp.Id))
				{
					randomPlanets.TryAdd(rp.Id, rp);
				}
				else
				{
					i--;
				}
			}
			return randomPlanets;
        }

        public DbPlanet GetRandomPlanet()
		{
			Random random = new Random();
			var i = random.Next(0, this.Count-1);
			return this.Values.ElementAt(i);
		}
	}
}
