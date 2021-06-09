using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Shopping.API.Repositories
{
    public interface IMarketPlaceRepository
    {
        GalaxyDbContext Context { get; }
        Task<MarketPlace> GetItem(Guid id);
        Task<MarketPlace> GetItemByName(string name);
        Task<List<MarketPlace>> GetItems();
    }

    public class MarketPlaceRepository : IMarketPlaceRepository
    {
        public GalaxyDbContext Context { get; }

        public MarketPlaceRepository(GalaxyDbContext context)
        {
            Context = context;
        }

        public async Task<List<MarketPlace>> GetItems()
        {
            return await Context.MarketPlaces
						 .Include(demands => demands.ProductDemands)
                   		 .ThenInclude(catItems => catItems.CatalogItems)
						 .ThenInclude(product => product.Product)
                   		 .Include(offerings => offerings.ProductOfferings)
                   		 .ThenInclude(catItems => catItems.CatalogItems)
						 .ThenInclude(product => product.Product)
                   		 .ToListAsync();
        }

        public async Task<MarketPlace> GetItem(Guid id)
        {
            return await Context.MarketPlaces
							.Include(demands => demands.ProductDemands)
							.ThenInclude(catItems => catItems.CatalogItems)
							.ThenInclude(product => product.Product)
							.Include(offerings => offerings.ProductOfferings)
							.ThenInclude(catItems => catItems.CatalogItems)
							.ThenInclude(product => product.Product)
							.FirstOrDefaultAsync(x => x.ID.Equals(id));
        }

        public async Task<MarketPlace> GetItemByName(string name)
        {
            return await Context.MarketPlaces.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }

    }
}
