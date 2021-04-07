using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.Galaxy.Creator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{
	public class CatalogItemsRepository : IRepository<CatalogItem>
	{
		public GalaxyDbContext Context { get; }

		public CatalogItemsRepository(GalaxyDbContext context)
		{
			Context = context;
		}


		public async Task<bool> AddItem(CatalogItem item)
		{
			var ci = await Context.CatalogItems.FirstOrDefaultAsync(x => x.Market.ID.Equals(item.Market.ID) & x.Product.Name.Equals(item.Product.Name));
			if (ci == null)
			{
				Context.CatalogItems.Add(item);
				await Context.SaveChangesAsync();
			}
			else
			{
				var result = UpdateItem(item);
			}
			return true;
		}

		public async Task<bool>  DeleteItem(CatalogItem item)
		{
			var itm = await Context.CatalogItems.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				Context.CatalogItems.Remove(itm);
				await Context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<CatalogItem> GetItem(Guid id)
		{
			return await Context.CatalogItems.FirstOrDefaultAsync(x => x.ID.Equals(id));
		}

		public async Task<CatalogItem> GetItemByName(string name)
		{
			return await Context.CatalogItems.FirstOrDefaultAsync(x => x.Title.Equals(name));
		}

		public async Task<List<CatalogItem>> GetItems()
		{
			return await Context.CatalogItems.ToListAsync();
		}

		public async Task<CatalogItem> UpdateItem(CatalogItem item)
		{
			var itm = await Context.CatalogItems.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				itm.Price = item.Price;
				itm.Product = item.Product;
				itm.Title = item.Title;
				await Context.SaveChangesAsync();
			}
			return item;
		}

	}
}
