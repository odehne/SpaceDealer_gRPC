using Cope.SpaceRogue.Galaxy.Creator.API.Domain;
using Galaxy.API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{
	public class PlayerRepository : IRepository<Player>
	{
		public GalaxyDbContext Context { get; }

		public PlayerRepository(GalaxyDbContext context)
		{
			Context = context;
		}
		public void AddItem(Player item)
		{
			if (item.ID == default)
				throw new ArgumentException("Product must have an Id.");

			if (string.IsNullOrEmpty(item.Name))
				throw new ArgumentException("Product must have a name.");
			var pn = GetItemByName(item.Name);
			if (pn == null)
			{
				Context.Players.Add(item);
				Context.SaveChanges();
			}
			else
			{
				UpdateItem(item);
			}
		}

		public void DeleteItem(Player item)
		{
			var itm = Context.Players.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				if (itm.Fleet.Any())
				{
					var shipRep = new ShipRepository(Context);
					foreach (var s in itm.Fleet)
					{
						shipRep.DeleteItem(s);
					}
				}
				Context.Players.Remove(itm);
				Context.SaveChanges();
			}
		}

		public Player GetItem(Guid id)
		{
			return Context.Players.FirstOrDefault(x => x.ID.Equals(id));
		}

		public Player GetItemByName(string name)
		{
			return Context.Players
				.Include(x=>x.Fleet)
				.ThenInclude(x=>x.Features)
				.FirstOrDefault(x => x.Name.Equals(name));
		}

		public List<Player> GetItems()
		{
			return Context.Players.ToList();
		}

		public Player UpdateItem(Player item)
		{
			throw new NotImplementedException();
		}

		public void DeleteMany(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
