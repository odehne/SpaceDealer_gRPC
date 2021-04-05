using Cope.SpaceRogue.Galaxy.Creator.API.Domain;
using Galaxy.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{
	public class ShipRepository : IRepository<Ship>
	{
		public GalaxyDbContext Context { get; }

		public ShipRepository(GalaxyDbContext context)
		{
			Context = context;
		}

		public void AddItem(Ship item)
		{
			if (item.ID == default)
				throw new ArgumentException("Ship must have an Id.");

			if (string.IsNullOrEmpty(item.Name))
				throw new ArgumentException("Ship must have a name.");

			var pn = GetItemByName(item.Name);
			if (pn == null)
			{
				Context.Ships.Add(item);
				Context.SaveChanges();
			}
			else
			{
				UpdateItem(item);
			}
		}

		public void DeleteItem(Ship item)
		{
			var itm = Context.Ships.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				Context.Ships.Remove(itm);
				Context.SaveChanges();
			}
		}

		public Ship GetItem(Guid id)
		{
			return Context.Ships.FirstOrDefault(x => x.ID.Equals(id));
		}

		public Ship GetItemByName(string name)
		{
			return Context.Ships.FirstOrDefault(x => x.Name.Equals(name));
		}

		public List<Ship> GetItems()
		{
			return Context.Ships.ToList();
		}

		public void UpdateHull(int id, int newHullValue)
		{
			var itm = Context.Ships.FirstOrDefault(x => x.ID.Equals(id));
			if (itm != null)
			{
				itm.Hull = newHullValue;
				Context.SaveChanges();
			}
		}

		public void UpdateShields(int id, int newShieldValue)
		{
			var itm = Context.Ships.FirstOrDefault(x => x.ID.Equals(id));
			if (itm != null)
			{
				itm.Shields = newShieldValue;
				Context.SaveChanges();
			}
		}

		public void UpdateCargo(int id, List<Payload> cargo)
		{
			var itm = Context.Ships.FirstOrDefault(x => x.ID.Equals(id));
			if (itm != null)
			{
				itm.Cargo = cargo;
				Context.SaveChanges();
			}
		}


		public Ship UpdateItem(Ship item)
		{
			var itm = Context.Ships.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				itm.Name = item.Name;
				itm.Cargo = item.Cargo;
				itm.Features = item.Features;
				itm.Hull = item.Hull;
				itm.Shields = item.Shields;
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
