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
	}
}
