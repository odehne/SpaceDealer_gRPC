using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Infrastructure.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.InfraStructure
{
	public class GalaxyRepository : IRepository<Planet>
	{
		public List<Planet> Planets { get; set; }

		public IPersister<Planet> Persister { get; set; }

		public async Task<Planet> GetItem(string itemId)
		{
			if (Planets.Count == 0)
			{
				Planets = await Persister.Read();
			}
			return Planets.FirstOrDefault(x => x.PlanetId.Equals(itemId));
		}

		public async Task<List<Planet>> GetItemsByName(string name)
		{
			if (Planets.Count == 0)
			{
				Planets = await Persister.Read();
			}
			return Planets.Where(x => x.Name.Contains(name, System.StringComparison.InvariantCultureIgnoreCase)).ToList();
		}

		public async Task<List<Planet>> Read()
		{
			if (Planets.Count == 0)
			{
				Planets = await Persister.Read();
			}
			return Planets;
		}

		public async Task<bool> Save()
		{
			return await Persister.Write(Planets);
		}
	}
}
