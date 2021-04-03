using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Galaxy.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{
	public class ProductGroupRepository : IRepository<ProductGroup>
	{
		public GalaxyDbContext Context { get; }

		public ProductGroupRepository(GalaxyDbContext context)
		{
			Context = context;
		}


		public void AddItem(ProductGroup item)
		{
			if (item.ID == default)
				throw new ArgumentException("Product group must have an Id.");

			if (string.IsNullOrEmpty(item.Name))
				throw new ArgumentException("Product group must have a name.");
			var pn = GetItemByName(item.Name);
			if (pn == null)
			{
				Context.ProductGroups.Add(item);
				Context.SaveChanges();
			}
		}

		public void DeleteItem(ProductGroup item)
		{
			var itm = Context.ProductGroups.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				Context.ProductGroups.Remove(itm);
				Context.SaveChanges();
			}
		}

		public void DeleteMany(Guid id)
		{
			throw new NotImplementedException();
		}

		public ProductGroup GetItem(Guid id)
		{
			return Context.ProductGroups.FirstOrDefault(x => x.ID.Equals(id));
		}

		public ProductGroup GetItemByName(string name)
		{
			return Context.ProductGroups.FirstOrDefault(x => x.Name.Equals(name));
		}

		public List<ProductGroup> GetItems()
		{
			return Context.ProductGroups.ToList();
		}

		public ProductGroup UpdateItem(ProductGroup item)
		{
			var itm = Context.ProductGroups.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				itm.Name = item.Name;
				Context.SaveChanges();
			}
			return item;
		}

		public void AddDefaults()
		{
			var metal = new ProductGroup("Metallverarbeitung");
			var food = new ProductGroup("Food");
			var material = new ProductGroup("Baumaterial");

			AddItem(metal);
			AddItem(food);
			AddItem(material);
		}
	}
}
