using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Galaxy.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{
	public class CatalogItemsRepository : IRepository<CatalogItem>
	{
		public GalaxyDbContext Context { get; }

		public CatalogItemsRepository(GalaxyDbContext context)
		{
			Context = context;
		}


		public void AddItem(CatalogItem item)
		{
			var ci = Context.CatalogItems.FirstOrDefault(x => x.Market.ID.Equals(item.Market.ID) & x.Product.Name.Equals(item.Product.Name));
			if (ci == null)
			{
				Context.CatalogItems.Add(item);
				Context.SaveChanges();
			}
		}

		public void DeleteItem(CatalogItem item)
		{
			var itm = Context.CatalogItems.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				Context.CatalogItems.Remove(itm);
				Context.SaveChanges();
			}
		}

		public CatalogItem GetItem(Guid id)
		{
			return Context.CatalogItems.FirstOrDefault(x => x.ID.Equals(id));
		}

		public CatalogItem GetItemByName(string name)
		{
			return Context.CatalogItems.FirstOrDefault(x => x.Title.Equals(name));
		}

		public List<CatalogItem> GetItems()
		{
			return Context.CatalogItems.ToList();
		}

		public CatalogItem UpdateItem(CatalogItem item)
		{
			var itm = Context.CatalogItems.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				itm.Price = item.Price;
				itm.Product = item.Product;
				itm.Title = item.Title;
				Context.SaveChanges();
			}
			return item;
		}

		public void DeleteMany(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
