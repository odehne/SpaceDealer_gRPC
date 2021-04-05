using Cope.SpaceRogue.Galaxy.Creator.API.Domain;
using Galaxy.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{

	public class FeatureRepository : IRepository<Feature>
	{
		public GalaxyDbContext Context { get; }

		public FeatureRepository(GalaxyDbContext context)
		{
			Context = context;
		}
		public void AddItem(Feature item)
		{
			if (item.ID == default)
				throw new ArgumentException("Feature must have an Id.");

			if (string.IsNullOrEmpty(item.Name))
				throw new ArgumentException("Feature must have a name.");

			var pn = GetItemByName(item.Name);
			if (pn == null)
			{
				Context.Features.Add(item);
				Context.SaveChanges();
			}
			else
			{
				UpdateItem(item);
			}
		}

		public void DeleteItem(Feature item)
		{
			var itm = Context.Features.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				Context.Features.Remove(itm);
				Context.SaveChanges();
			}
		}

		public Feature GetItem(Guid id)
		{
			return Context.Features.FirstOrDefault(x => x.ID.Equals(id));
		}

		public Feature GetItemByName(string name)
		{
			return Context.Features.FirstOrDefault(x => x.Name.Equals(name));
		}

		public List<Feature> GetItems()
		{
			return Context.Features.ToList();
		}

		public Feature UpdateItem(Feature item)
		{
			var itm = Context.Features.FirstOrDefault(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				itm.Name = item.Name;
				itm.Description = item.Description;
				itm.BattleAdvantage = item.BattleAdvantage;
				itm.BattleDisadvantage = item.BattleDisadvantage;
				itm.FreightCapacityAdvantage = item.FreightCapacityAdvantage;
				itm.FreightCapacityDisadvantage = item.FreightCapacityDisadvantage;
				itm.SensorRangeAdvantage = item.SensorRangeAdvantage;
				Context.SaveChanges();
			}
			return item;
		}

		public void DeleteMany(Guid id)
		{
			throw new NotImplementedException();
		}

		public void AddDefaults()
		{
			var sensor = new Feature("Erweiterter Sensorik", "Kann andere Schiffe und Planeten schneller erkennen.", 0,0,0,0, 1);
			var battle = new Feature("Verbesserte Laserbänke", "Angriff +1.", 1,0,0,0,0);
			var cargo = new Feature("Vergrösserter Frachtraum", "Frachtraum vergößert um 10 Tonnen.",0,0,10,0,0);
			
			AddItem(sensor);
			AddItem(battle);
			AddItem(cargo);
		}
	}
}
