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
		Task<double> Deposit(Guid id, double newValue);
        Task<double> Withdraw(Guid id, double newValue);

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

        public async Task<double> Deposit(Guid id, double amount)
        {
            var player = await Context.Players.FirstOrDefaultAsync(x => x.ID.Equals(id));
            if (player != null)
            {
                var balance = (double)player.Credits;
                var newBalance = balance + amount;
                player.Credits = (decimal)newBalance;
                await Context.SaveChangesAsync();
                return newBalance;
            }
            return 0.0;
        }

        public async Task<double> Withdraw(Guid id, double amount)
        {
            var player = await Context.Players.FirstOrDefaultAsync(x => x.ID.Equals(id));
            if (player != null)
            {
                if (amount < 0)
                    amount *= -1;
                
                var balance = (double)player.Credits;
                if (balance < amount)
                    throw new InvalidOperationException("Not enough credits.");

                var newBalance = balance - amount;

			    player.Credits = (decimal)newBalance;
                Context.SaveChanges();
                return newBalance;

            }
            return 0.0;
        }
    }
}
