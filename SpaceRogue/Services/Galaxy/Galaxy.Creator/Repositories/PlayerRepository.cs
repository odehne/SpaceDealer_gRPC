using Cope.SpaceRogue.Galaxy.Creator.API.Domain;
using Galaxy.API.Domain;
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
			throw new NotImplementedException();
		}

		public void DeleteItem(Player item)
		{
			var itm = Context.Players.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				if (itm.Fleet.Any())
				{
					var shipRep = new ShipRepository(Context);
					shipRep.DeleteMany(itm.ID);
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
			return Context.Players.FirstOrDefault(x => x.Name.Equals(name));
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
