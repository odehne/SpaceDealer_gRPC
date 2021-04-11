using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Galaxy.API.Domain;
using Cope.SpaceRogue.Galaxy.Creator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Repositories
{
    public interface IPlanetRepository
    {
        GalaxyDbContext Context { get; }

        Task<bool> AddDefaults();
        Task<bool> AddItem(Planet item);
        Task<bool> DeleteItem(Planet item);
        Task<Planet> GetItem(Guid id);
        Task<Planet> GetItemByName(string name);
        Task<List<Planet>> GetItems();
        Task<Planet> UpdateItem(Planet item);
    }

    public class PlanetRepository : IPlanetRepository
    {
        public GalaxyDbContext Context { get; }

        public PlanetRepository(GalaxyDbContext context)
        {
            Context = context;
        }

        public async Task<List<Planet>> GetItems()
        {
            return await Context.Planets
                    .Include(x => x.Market)
                    .Include(x => x.Market.ProductDemands)
                    .Include(x => x.Market.ProductOfferings)
                    .Include(x => x.Market.ProductDemands.CatalogItems)
                    .Include(x => x.Market.ProductOfferings.CatalogItems)
                    .ToListAsync();
        }

        public async Task<Planet> GetItem(Guid id)
        {
            return await Context.Planets
                    .Include(x => x.Market)
					.FirstOrDefaultAsync(x => x.ID.Equals(id));
        }

        public async Task<Planet> GetItemByName(string name)
        {
            return await Context.Planets.Include(x => x.Market).FirstOrDefaultAsync(y => y.Name.Equals(name));
        }

        public async Task<Planet> UpdateItem(Planet item)
        {
            var itm = await Context.Planets.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (itm != null)
            {
                itm.Name = item.Name;
                itm.Description = item.Description;
                itm.PosX = item.PosX;
                itm.PosY = item.PosY;
                itm.PosZ = item.PosZ;
                Context.SaveChanges();
            }
            return item;
        }

        public async Task<bool> AddItem(Planet item)
        {
            if (item.ID == default)
                throw new ArgumentException("Planet must have an Id.");

            if (string.IsNullOrEmpty(item.Name))
                throw new ArgumentException("Planet must have a name.");

            var pn = await GetItemByName(item.Name);
            if (pn == null)
            {
                Context.Planets.Add(item);
                await Context.SaveChangesAsync();
            }
            else
            {
                var p = await UpdateItem(item);
            }
            return true;
        }

        public async Task<bool> DeleteItem(Planet item)
        {
            var itm = await Context.Planets.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (itm != null)
            {
                Context.Planets.Remove(itm);
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> AddDefaults()
        {
            var offerings = new Catalog();
            var demands = new Catalog();
            var productRepo = new ProductRepository(Context);

            offerings.AddCatalogItem(await productRepo.GetItemByName("Milch"), "Kuh-Milch", -15);
            offerings.AddCatalogItem(await productRepo.GetItemByName("Wasser"), "Gletscherwasser", -25);
            offerings.AddCatalogItem(await productRepo.GetItemByName("Mehl"), "Weizen-Mehl", -5);

            demands.AddCatalogItem(await productRepo.GetItemByName("Holz"), "Holz", 5);
            demands.AddCatalogItem(await productRepo.GetItemByName("Wasser"), "Gletscherwasser", 11);
            demands.AddCatalogItem(await productRepo.GetItemByName("Mehl"), "Weizen-Mehl", 15);

            var market = new MarketPlace("Marktplatz New New York", offerings, demands);
            var marketRepo = new MarketPlaceRepository(Context);
            var b = await marketRepo.AddItem(market);

            var planet = new Planet(market, "Erde", "Der blaue Planet", 0, 0, 0);

            return await AddItem(planet);

        }
    }
}