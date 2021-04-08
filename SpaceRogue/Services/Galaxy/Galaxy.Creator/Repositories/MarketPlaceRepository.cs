using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.Galaxy.Creator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{
    public interface IMarketPlaceRepository
    {
        GalaxyDbContext Context { get; }

        Task<bool> AddItem(MarketPlace item);
        Task<bool> DeleteItem(MarketPlace item);
        Task<MarketPlace> GetItem(Guid id);
        Task<MarketPlace> GetItemByName(string name);
        Task<List<MarketPlace>> GetItems();
        Task<MarketPlace> UpdateItem(MarketPlace item);
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
            return await Context.MarketPlaces.ToListAsync();
        }

        public async Task<MarketPlace> GetItem(Guid id)
        {
            return await Context.MarketPlaces.FirstOrDefaultAsync(x => x.ID.Equals(id));
        }

        public async Task<MarketPlace> GetItemByName(string name)
        {
            return await Context.MarketPlaces.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }

        public async Task<MarketPlace> UpdateItem(MarketPlace item)
        {
            var ci = await Context.MarketPlaces.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (ci != null)
            {
                ci.Name = item.Name;
                ci.ProductDemands = item.ProductDemands;
                ci.ProductOfferings = item.ProductOfferings;
                await Context.SaveChangesAsync();
            }
            return item;
        }

        public async Task<bool> AddItem(MarketPlace item)
        {
            var ci = await Context.MarketPlaces.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (ci == null)
            {
                Context.MarketPlaces.Add(item);
                await Context.SaveChangesAsync();
            }
            else
            {
                var result = UpdateItem(item);
            }
            return true;
        }

        public async Task<bool> DeleteItem(MarketPlace item)
        {
            var itm = await Context.MarketPlaces.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (itm != null)
            {
                Context.MarketPlaces.Remove(itm);
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
