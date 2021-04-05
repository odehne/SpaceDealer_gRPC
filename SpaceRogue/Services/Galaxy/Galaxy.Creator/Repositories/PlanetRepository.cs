using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Galaxy.Creator.API.Domain;
using Galaxy.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{

	public class PlanetRepository : IRepository<Planet>
	{
		public GalaxyDbContext Context { get; }

		public PlanetRepository(GalaxyDbContext context)
		{
			Context = context;
		}

		public List<Planet> GetItems()
		{
			return Context.Planets.ToList();
		}

		public Planet GetItem(Guid id)
		{
			return Context.Planets.FirstOrDefault(x => x.ID.Equals(id));
		}

		public Planet GetItemByName(string name)
		{
			return Context.Planets.FirstOrDefault(x => x.Name.Equals(name));
		}

		public Planet UpdateItem(Planet item)
		{
			var itm = Context.Planets.FirstOrDefault(x => x.ID.Equals(item.ID));
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

		public void AddItem(Planet item)
		{
			if (item.ID == default)
				throw new ArgumentException("Planet must have an Id.");

			if (string.IsNullOrEmpty(item.Name))
				throw new ArgumentException("Planet must have a name.");

			var pn = GetItemByName(item.Name);
			if (pn == null)
			{
				Context.Planets.Add(item);
				Context.SaveChanges();
			}
		}

		public void DeleteItem(Planet item)
		{
			var itm = Context.Planets.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				Context.Planets.Remove(itm);
				Context.SaveChanges();
			}
		}

		public void DeleteMany(Guid id)
		{
			throw new NotImplementedException();
		}

		public void AddDefaults()
		{
			var prodRep= new ProductRepository(Context);
			var offerings = new Catalog();
			var demands = new Catalog();

			offerings.AddCatalogItem(prodRep.GetItemByName("Eisen"), "Eisen", 7.00);


			var market = new MarketPlace();


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
	}
}
