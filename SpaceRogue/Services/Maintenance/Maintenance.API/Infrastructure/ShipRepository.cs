using Cope.SpaceRogue.Infrastructure.Repository;
using Cope.SpaceRogue.Maintenance.API.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Maintenance.API.Infrastructure
{
	public class ShipRepository : IRepository<Ship>
	{
		public List<Ship> Ships { get; set; }

		public IPersister<Ship> Persister { get; set; }

		public async Task<Ship> GetItem(string itemId)
		{
			if (Ships.Count == 0)
			{
				Ships = await Persister.Read();
			}
			return Ships.FirstOrDefault(x => x.ShipId.Equals(itemId));
		}

		public async Task<List<Ship>> GetItemsByName(string name)
		{
			if (Ships.Count == 0)
			{
				Ships = await Persister.Read();
			}
			return Ships.Where(x => x.Name.Contains(name, System.StringComparison.InvariantCultureIgnoreCase)).ToList();
		}

		public async Task<List<Ship>> Read()
		{
			if (Ships.Count == 0)
			{
				Ships = await Persister.Read();
			}
			return Ships;
		}

		public async Task<bool> Save()
		{
			return await Persister.Write(Ships);
		}
	}
}