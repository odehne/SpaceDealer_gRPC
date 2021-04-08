using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.Galaxy.Creator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{
    public interface IProductRepository
    {
        GalaxyDbContext Context { get; }

        Task<bool> AddDefaults();
        Task<bool> AddItem(Product item);
        Task<bool> DeleteItem(Product item);
        Task<Product> GetItem(Guid id);
        Task<Product> GetItemByName(string name);
        Task<List<Product>> GetItems();
        Task<Product> UpdateItem(Product item);
    }

    public class ProductRepository : IProductRepository
    {
        public GalaxyDbContext Context { get; }

        public ProductRepository(GalaxyDbContext context)
        {
            Context = context;
        }

        public async Task<bool> DeleteItem(Product item)
        {
            var itm = await Context.Products.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (itm != null)
            {
                Context.Products.Remove(itm);
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Product> GetItem(Guid id)
        {
            return await Context.Products.FirstOrDefaultAsync(x => x.ID.Equals(id));
        }

        public async Task<Product> GetItemByName(string name)
        {
            return await Context.Products.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }

        public async Task<List<Product>> GetItems()
        {
            return await Context.Products.ToListAsync();
        }

        public async Task<Product> UpdateItem(Product item)
        {
            var itm = await Context.Products.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (itm != null)
            {
                itm.Name = item.Name;
                itm.PricePerUnit = item.PricePerUnit;
                itm.Group = item.Group;
                itm.Rarity = item.Rarity;
                itm.SizeInUnits = item.SizeInUnits;
                await Context.SaveChangesAsync();
            }
            return item;
        }

        public async Task<bool> AddItem(Product item)
        {
            if (item.ID == default)
                throw new ArgumentException("Product must have an Id.");

            if (string.IsNullOrEmpty(item.Name))
                throw new ArgumentException("Product must have a name.");
            var pn = await GetItemByName(item.Name);
            if (pn == null)
            {
                Context.Products.Add(item);
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> AddDefaults()
        {
            var groupsRep = new ProductGroupRepository(Context);
            var metal = await groupsRep.GetItemByName("Metallverarbeitung");
            var food = await groupsRep.GetItemByName("Food");
            var material = await groupsRep.GetItemByName("Baumaterial");

            var ps = new Product[]
            {
                new Product("Eisen", metal.ID, 1.0, 600.0, 10.0 ),
                new Product("Stahl", metal.ID, 1.0, 570.0, 80.0 ),
                new Product("Kupfer", metal.ID, 1.0, 570.0, 80.0 ),
                new Product("Silber", metal.ID, 1.0, 60.0, 1000.0 ),
                new Product("Gold", metal.ID, 1.0, 60.0, 10000.0 ),
                new Product("Messing", metal.ID, 1.0, 600.0, 10.0 ),
                new Product("Fisch", food.ID, 1.0, 300.0, 100.0 ),
                new Product("Milch", food.ID, 1.0, 5000.0, 15.0 ),
                new Product("Mehl", food.ID, 1.0, 5000.0, 15.0 ),
                new Product("Wasser", food.ID, 1.0, 1000.0, 15.0 ),
                new Product("Holz", material.ID, 1.0, 1000.0, 45.0 ),
                new Product("Granit", material.ID, 3.0, 1000.0, 15.0 ),
                new Product("Mamor", material.ID, 3.0, 700.0, 65.0 )
            };

            foreach (var p in ps)
            {
                var b = AddItem(p);
            }
            return true;
        }

    }
}
