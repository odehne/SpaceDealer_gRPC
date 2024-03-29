﻿using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Repositories
{
    public interface IPlayerRepository
    {
        GalaxyDbContext Context { get; }

        Task<bool> AddItem(Player item);
        Task<bool> DeleteItem(Player item);
        Task<Player> GetItem(Guid id);
        Task<Player> GetItemByName(string name);
        Task<List<Player>> GetItems();
        Task<Player> UpdateItem(Player item);
    }

    public class PlayerRepository : IPlayerRepository
    {
        public GalaxyDbContext Context { get; }

        public PlayerRepository(GalaxyDbContext context)
        {
            Context = context;
        }
        public async Task<bool> AddItem(Player item)
        {
            if (item.ID == default)
                throw new ArgumentException("Product must have an Id.");

            if (string.IsNullOrEmpty(item.Name))
                throw new ArgumentException("Product must have a name.");
            var pn = await GetItemByName(item.Name);
            if (pn == null)
            {
                Context.Players.Add(item);
                await Context.SaveChangesAsync();
            }
            else
            {
                var itm = await UpdateItem(item);
            }
            return true;
        }

        public async Task<bool> DeleteItem(Player item)
        {
            var itm = await Context.Players.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (itm != null)
            {
                if (itm.Fleet.Any())
                {
                    var shipRep = new ShipRepository(Context);
                    foreach (var s in itm.Fleet)
                    {
                        await shipRep.DeleteItem(s);
                    }
                }
                Context.Players.Remove(itm);
                await Context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Player> GetItem(Guid id)
        {
            return await Context.Players
                    .Include( x=> x.Fleet)
                    .ThenInclude ( x=>x.Features)
                        .FirstOrDefaultAsync(x => x.ID.Equals(id));
        }

        public async Task<Player> GetItemByName(string name)
        {
            return await Context.Players
                .Include(x => x.Fleet)
                .ThenInclude(x => x.Features)
                .FirstOrDefaultAsync(x => x.Name.Equals(name));
        }

        public async Task<List<Player>> GetItems()
        {
            return await Context.Players
            .Include(x => x.Fleet)
                .ThenInclude(x => x.Features)
                .ToListAsync();
        }

        public async Task<Player> UpdateItem(Player item)
        {
            var itm = await Context.Players.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
            if (itm != null)
            {
                itm.Name = item.Name;
                itm.Credits = item.Credits;
                itm.HomePlanet = item.HomePlanet;
                itm.PlayerType = item.PlayerType;
                itm.Fleet = item.Fleet;
                Context.SaveChanges();
            }
            return item;
        }

    }
}
