using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Domain;
using Cope.SpaceRogue.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API.Repositories
{
    public interface IPlanetRepository
    {
        GalaxyDbContext Context { get; }
        Task<Planet> GetItem(Guid id);
        Task<List<Planet>> GetItems();
        Task<Planet> GetPlanetByPosition(int posX, int posY, int pos);

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
                    .ThenInclude(pd => pd.CatalogItems)
                    .Include(x => x.Market.ProductOfferings)
                    .ThenInclude(off => off.CatalogItems)
                    .ToListAsync();
        }

        public async Task<Planet> GetItem(Guid id)
        {
            return await Context.Planets
                    .Include(x => x.Market)
					.FirstOrDefaultAsync(x => x.ID.Equals(id));
        }

        public async Task<Planet> GetPlanetByPosition(int posX, int posY, int posZ)
        {
            return await Context.Planets
                   .Include(x => x.Market)
                   .FirstOrDefaultAsync(x => x.PosX == posX & x.PosY == posY & x.PosZ == posZ);
        }
    }
}