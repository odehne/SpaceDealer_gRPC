using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Shopping.API.Repositories
{
    public interface IProductRepository
    {
        GalaxyDbContext Context { get; }
        Task<Product> GetItem(Guid id);
        Task<Product> GetItemByName(string name);
        Task<List<Product>> GetItems();
    }

    public class ProductRepository : IProductRepository
	{
        public GalaxyDbContext Context { get; }

        public ProductRepository(GalaxyDbContext context)
        {
            Context = context;
        }

        public async Task<Product> GetItem(Guid id)
        {
            return await Context.Products.Include(x => x.Group).FirstOrDefaultAsync(x => x.ID.Equals(id));
        }

        public async Task<Product> GetItemByName(string name)
        {
            return await Context.Products.Include(x => x.Group).FirstOrDefaultAsync(x => x.Name.Equals(name));
        }

        public async Task<List<Product>> GetItems()
        {
            return await Context.Products
                        .Include(x => x.Group)
                        .ToListAsync();
        }

    }

}
