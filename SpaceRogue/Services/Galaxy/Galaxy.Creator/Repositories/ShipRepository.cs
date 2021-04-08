using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.Galaxy.Creator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{
    public interface IShipRepository
    {
        GalaxyDbContext Context { get; }
        Task<bool> AddItem(Ship item);
        Task<bool> DeleteItem(Ship item);
        Task<Ship> GetItem(Guid id);
        Task<Ship> GetItemByName(string name);
        Task<List<Ship>> GetItems();
        Task<bool> UpdateCargo(int id, List<Payload> cargo);
        Task<bool> UpdateHull(int id, int newHullValue);
        Task<Ship> UpdateItem(Ship item);
        Task<bool> UpdateShields(int id, int newShieldValue);
    }

    public class ShipRepository : IShipRepository
    {
        public GalaxyDbContext Context { get; }

        public ShipRepository(GalaxyDbContext context)
        {
            Context = context;
        }

        public async Task<bool> AddItem(Ship item)
        {
            if (item.ID == default)
                throw new ArgumentException("Ship must have an Id.");

            if (string.IsNullOrEmpty(item.Name))
                throw new ArgumentException("Ship must have a name.");

            var pn = await GetItemByName(item.Name);
            if (pn == null)
            {
                Context.Ships.Add(item);
                await Context.SaveChangesAsync();
            }
            else
            {
                await UpdateItem(item);
            }
            return true;
        }

        public async Task<bool> DeleteItem(Ship item)
        {
            var itm = await Context.Ships.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (itm != null)
            {
                Context.Ships.Remove(itm);
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Ship> GetItem(Guid id)
        {
            return await Context.Ships.FirstOrDefaultAsync(x => x.ID.Equals(id));
        }

        public async Task<Ship> GetItemByName(string name)
        {
            return await Context.Ships.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }

        public async Task<List<Ship>> GetItems()
        {
            return await Context.Ships.ToListAsync();
        }

        public async Task<bool> UpdateHull(int id, int newHullValue)
        {
            var itm = await Context.Ships.FirstOrDefaultAsync(x => x.ID.Equals(id));
            if (itm != null)
            {
                itm.Hull = newHullValue;
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateShields(int id, int newShieldValue)
        {
            var itm = await Context.Ships.FirstOrDefaultAsync(x => x.ID.Equals(id));
            if (itm != null)
            {
                itm.Shields = newShieldValue;
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateCargo(int id, List<Payload> cargo)
        {
            var itm = await Context.Ships.FirstOrDefaultAsync(x => x.ID.Equals(id));
            if (itm != null)
            {
                itm.Cargo = cargo;
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<Ship> UpdateItem(Ship item)
        {
            var itm = await Context.Ships.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (itm != null)
            {
                itm.Name = item.Name;
                itm.Cargo = item.Cargo;
                itm.Features = item.Features;
                itm.Hull = item.Hull;
                itm.Shields = item.Shields;
                await Context.SaveChangesAsync();
            }
            return item;
        }
    }
}
