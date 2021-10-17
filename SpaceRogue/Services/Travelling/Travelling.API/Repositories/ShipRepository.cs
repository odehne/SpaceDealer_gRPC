using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Domain;
using Cope.SpaceRogue.Travelling.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API.Repositories
{
	public interface IShipRepository
    {
        GalaxyDbContext Context { get; }
        Task<Ship> GetItem(Guid id);
        Task<List<Ship>> GetItems();
        Task<bool> UpdatePosition(Ship item);
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
            return await Context.Ships.FirstOrDefaultAsync(x => x.Id.Equals(id));
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

        public async Task<bool> UpdatePosition(Ship item)
        {
            var itm = await Context.Ships.FirstOrDefaultAsync(x => x.Id.Equals(item.Id));
            if (itm != null)
            {
        		itm.CurrentPosX = item.CurrentPosX;
                itm.CurrentPosY = item.CurrentPosY;
                itm.CurrentPosZ = item.CurrentPosZ;
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
