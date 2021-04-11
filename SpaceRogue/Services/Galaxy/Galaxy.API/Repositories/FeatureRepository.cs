using Cope.SpaceRogue.Galaxy.API.Domain;
using Cope.SpaceRogue.Galaxy.API;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Repositories
{
    public interface IFeatureRepository
    {
        GalaxyDbContext Context { get; }

        Task<bool> AddDefaults();
        Task<bool> AddItem(Feature item);
        Task<bool> DeleteItem(Feature item);
        Task<Feature> GetItem(Guid id);
        Task<Feature> GetItemByName(string name);
        Task<List<Feature>> GetItems();
        Task<Feature> UpdateItem(Feature item);
    }

    public class FeatureRepository : IFeatureRepository
    {
        public GalaxyDbContext Context { get; }

        public FeatureRepository(GalaxyDbContext context)
        {
            Context = context;
        }
        public async Task<bool> AddItem(Feature item)
        {
            if (item.ID == default)
                throw new ArgumentException("Feature must have an Id.");

            if (string.IsNullOrEmpty(item.Name))
                throw new ArgumentException("Feature must have a name.");

            var pn = GetItemByName(item.Name);
            if (pn == null)
            {
                Context.Features.Add(item);
                await Context.SaveChangesAsync();
            }
            else
            {
                await UpdateItem(item);
            }
            return true;
        }

        public async Task<bool> DeleteItem(Feature item)
        {
            var itm = await Context.Features.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (itm != null)
            {
                Context.Features.Remove(itm);
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Feature> GetItem(Guid id)
        {
            return await Context.Features.FirstOrDefaultAsync(x => x.ID.Equals(id));
        }

        public async Task<Feature> GetItemByName(string name)
        {
            return await Context.Features.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }

        public async Task<List<Feature>> GetItems()
        {
            return await Context.Features.ToListAsync();
        }

        public async Task<Feature> UpdateItem(Feature item)
        {
            var itm = await Context.Features.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (itm != null)
            {
                itm.Name = item.Name;
                itm.Description = item.Description;
                itm.BattleAdvantage = item.BattleAdvantage;
                itm.BattleDisadvantage = item.BattleDisadvantage;
                itm.FreightCapacityAdvantage = item.FreightCapacityAdvantage;
                itm.FreightCapacityDisadvantage = item.FreightCapacityDisadvantage;
                itm.SensorRangeAdvantage = item.SensorRangeAdvantage;
                await Context.SaveChangesAsync();
            }
            return item;
        }

        public async Task<bool> AddDefaults()
        {
            var sensor = new Feature("Erweiterter Sensorik", "Kann andere Schiffe und Planeten schneller erkennen.", 0, 0, 0, 0, 1);
            var battle = new Feature("Verbesserte Laserbänke", "Angriff +1.", 1, 0, 0, 0, 0);
            var cargo = new Feature("Vergrösserter Frachtraum", "Frachtraum vergößert um 10 Tonnen.", 0, 0, 10, 0, 0);
            var b = false;
            b = await AddItem(sensor);
            b = await AddItem(battle);
            b = await AddItem(cargo);
            return b;
        }
    }
}
