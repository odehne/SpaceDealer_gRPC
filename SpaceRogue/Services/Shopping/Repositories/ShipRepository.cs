using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Shopping.API.Repositories
{
	public interface IShipRepository
    {
        GalaxyDbContext Context { get; }
        Task<Ship> GetItem(Guid id);
        Task<List<Ship>> GetItems();
        Task<bool> UnloadCargo(Guid shipId, Guid productId, double amount);
        Task<bool> LoadCargo(Guid shipId, Guid productId, double amount);
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

        public async Task<List<Ship>> GetItems()
        {
            return await Context.Ships.ToListAsync();
        }

		public async Task<bool> UnloadCargo(Guid shipId, Guid productId, double amount)
		{
			var ship = await Context.Ships
                                        .Include(x=>x.Cargo)
                                        .FirstOrDefaultAsync(x => x.ID.Equals(shipId));

            if (ship == null)
                throw new ArgumentException($"Ship with shipId {shipId} not found.");

            var payload = ship.Cargo.FirstOrDefault(x => x.Product.ID.Equals(productId));

            if(payload==null)
                throw new ArgumentException($"The requested product {productId} is not loaded.");


        }

        public async Task<bool> LoadCargo(Guid shipId, Guid productId, double amount)
		{
            var ship = await Context.Ships.Include(x => x.Cargo)
                                        .FirstOrDefaultAsync(x => x.ID.Equals(shipId));
            if (ship == null)
                throw new ArgumentException($"Ship with shipId {shipId} not found.");
        }
    }
}
