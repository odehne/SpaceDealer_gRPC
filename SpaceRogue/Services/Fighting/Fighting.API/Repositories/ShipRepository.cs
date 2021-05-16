using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Repositories
{

    public interface IFightRepository
    {
        GalaxyDbContext Context { get; }
        Task<Fight> GetItem(Guid id);
        Task<List<Fight>> GetItems();
    }

    public interface IShipRepository
    {
        GalaxyDbContext Context { get; }
        Task<Ship> GetItem(Guid id);
        Task<List<Ship>> GetItems();
    }

    public class ShipRepository : IShipRepository
    {
        public GalaxyDbContext Context { get; }

        public ShipRepository(GalaxyDbContext context)
        {
            Context = context;
        }

        public async Task<Ship> GetItem(Guid id)
        {
            return await Context.Ships.FirstOrDefaultAsync(x => x.ID.Equals(id));
        }

        public async Task<Ship> GetItemByName(string name)
        {
            return await Context.Ships.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }

        public async Task<List<Ship>> GetItems()
        {
            return await Context.Ships.ToListAsync();
        }

        public async Task<List<Ship>> GetItemsByPlayerId(Guid playerId)
        {
            var p = await Context.Players.Include(ships => ships.Fleet).FirstOrDefaultAsync(x => x.ID == playerId);
            return p.Fleet;
        }

		public Task<List<Ship>> GetItemsAtPosition(int posX, int posY, int posZ)
		{
			throw new NotImplementedException();
		}
	}
}
