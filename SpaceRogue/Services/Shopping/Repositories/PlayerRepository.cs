using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Shopping.API.Repositories
{

	public interface IPlayerRepository
	{
		GalaxyDbContext Context { get; }
		Task<Player> GetItem(Guid id);
		Task<List<Player>> GetItems();
		Task<bool> UpdateCredits(Guid id, double newValue);

	}

	public class PlayerRepository : IPlayerRepository
	{
        public GalaxyDbContext Context { get; }

        public PlayerRepository(GalaxyDbContext context)
        {
            Context = context;
        }

        public async Task<Player> GetItem(Guid id)
        {
            return await Context.Players
                    .Include(x => x.Fleet)
                        .ThenInclude(ft => ft.Features)
                    .Include(x => x.Fleet)
                        .ThenInclude(co => co.Cargo)
                    .FirstOrDefaultAsync(x => x.ID.Equals(id));
        }

        public async Task<Player> GetItemByName(string name)
        {
            return await Context.Players
                .Include(x => x.Fleet)
                    .ThenInclude(x => x.Features)
                .FirstOrDefaultAsync(x => x.Name.Equals(name));
        }

        public async Task<List<Player>> GetItems()
        {
            return await Context.Players
                .Include(x => x.Fleet)
                    .ThenInclude(x => x.Features)
                .ToListAsync();
        }

        public async Task<bool> UpdateCredits(Guid id, double newValue)
        {
            var itm = await Context.Players.FirstOrDefaultAsync(x => x.ID.Equals(id));
            if (itm != null)
            {
                itm.Credits = (decimal)newValue;
                Context.SaveChanges();
            }
            return true;
        }
	}
}
