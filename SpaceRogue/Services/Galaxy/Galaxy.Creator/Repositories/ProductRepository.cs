using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Galaxy.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{
	public class ProductRepository : IRepository<Product>
	{
		public GalaxyDbContext Context { get; }

		public ProductRepository(GalaxyDbContext context)
		{
			Context = context;
		}

		public void DeleteItem(Product item)
		{
			var itm = Context.Products.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				Context.Products.Remove(itm);
				Context.SaveChanges();
			}
		}

		public Product GetItem(Guid id)
		{
			return Context.Products.FirstOrDefault(x => x.ID.Equals(id));
		}

		public Product GetItemByName(string name)
		{
			return Context.Products.FirstOrDefault(x => x.Name.Equals(name));
		}

		public List<Product> GetItems()
		{
			return Context.Products.ToList();
		}

		public Product UpdateItem(Product item)
		{
			var itm = Context.Products.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				itm.Name = item.Name;
				itm.PricePerUnit= item.PricePerUnit;
				itm.Group= item.Group;
				itm.Rarity = item.Rarity;
				itm.SizeInUnits = item.SizeInUnits;
				Context.SaveChanges();
			}
			return item;
		}

		public void AddItem(Product item)
		{
			if (item.ID == default)
				throw new ArgumentException("Product must have an Id.");

			if (string.IsNullOrEmpty(item.Name))
				throw new ArgumentException("Product must have a name.");
			var pn = GetItemByName(item.Name);
			if (pn == null)
			{
				Context.Products.Add(item);
				Context.SaveChanges();
			}
		}

		public void AddDefaults()
		{
			var groupsRep = new ProductGroupRepository(Context);
			var metal = groupsRep.GetItemByName("Metallverarbeitung");
			var food = groupsRep.GetItemByName("Food");
			var material = groupsRep.GetItemByName("Baumaterial");
			
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
				AddItem(p);
			}
		}

		public void DeleteMany(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
