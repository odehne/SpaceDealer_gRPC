using Cope.SpaceRogue.Infrastructure.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Ship.API.Infrastructure
{
	public class ShipRepository : IRepository<Ship>
	{
		public List<Freighter> Freighters { get; set; }

		public IPersister<Freighter> Persister { get; set; }
		
		public async Task<Freighter> GetItem(string itemId)
		{
			if (Freighters.Count == 0)
			{
				Freighters = await Persister.Read();
			}
			return Freighters.FirstOrDefault(x => x.ShipId.Equals(itemId));
		}

		public async Task<List<Freighter>> Read()
		{
			if (Freighters.Count == 0)
			{
				Freighters = await Persister.Read();
			}
			return Freighters;
		}

		public async Task<bool> Save()
		{
			return await Persister.Write(Freighters);
		}
	}
}
