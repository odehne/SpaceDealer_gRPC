using Cope.SpaceRogue.Galaxy.Creator.API.Domain;
using Galaxy.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{

	public class MarketPlaceRepository : IRepository<MarketPlace>
	{
		public GalaxyDbContext Context { get; }

		public MarketPlaceRepository(GalaxyDbContext context)
		{
			Context = context;
		}

		public List<MarketPlace> GetItems()
		{
			return Context.MarketPlaces.ToList();
		}

		public MarketPlace GetItem(Guid id)
		{
			return Context.MarketPlaces.FirstOrDefault(x => x.ID.Equals(id));
		}

		public MarketPlace GetItemByName(string name)
		{
			return Context.MarketPlaces.FirstOrDefault(x => x.Name.Equals(name));
		}

		public MarketPlace UpdateItem(MarketPlace item)
		{
			var ci = Context.MarketPlaces.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (ci != null)
			{
				ci.Name = item.Name;
				ci.ProductDemands = item.ProductDemands;
				ci.ProductOfferings = item.ProductOfferings;
				Context.SaveChanges();
			}
			return item;
		}

		public void AddItem(MarketPlace item)
		{
			var ci = Context.MarketPlaces.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (ci == null)
			{
				Context.MarketPlaces.Add(item);
				Context.SaveChanges();
			}
			else
			{
				UpdateItem(item);
			}
		}

		public void DeleteItem(MarketPlace item)
		{
			var itm = Context.MarketPlaces.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				Context.MarketPlaces.Remove(itm);
				Context.SaveChanges();
			}
		}

		public void DeleteMany(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
