﻿using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.Galaxy.Creator;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{
	public class ProductGroupRepository : IRepository<ProductGroup>
	{
		public GalaxyDbContext Context { get; }

		public ProductGroupRepository(GalaxyDbContext context)
		{
			Context = context;
		}
		public async Task<bool> AddItem(ProductGroup item)
		{
			if (item.ID == default)
				throw new ArgumentException("Product group must have an Id.");

			if (string.IsNullOrEmpty(item.Name))
				throw new ArgumentException("Product group must have a name.");
			var pn = await GetItemByName(item.Name);
			if (pn == null)
			{
				Context.ProductGroups.Add(item);
				await Context.SaveChangesAsync();
			}
			else
			{
				var result = await UpdateItem(item);
			}
			return true;
		}

		public async Task<bool> DeleteItem(ProductGroup item)
		{
			var itm = await Context.ProductGroups.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				Context.ProductGroups.Remove(itm);
				await Context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<ProductGroup>  GetItem(Guid id)
		{
			return await Context.ProductGroups.FirstOrDefaultAsync(x => x.ID.Equals(id));
		}

		public async Task<ProductGroup> GetItemByName(string name)
		{
			return await Context.ProductGroups.FirstOrDefaultAsync(x => x.Name.Equals(name));
		}

		public async Task<List<ProductGroup>> GetItems()
		{
			return await Context.ProductGroups
					.Include(x=>x.Products)
					.ToListAsync();
		}

		public async Task<ProductGroup> UpdateItem(ProductGroup item)
		{
			var itm = await Context.ProductGroups.FirstOrDefaultAsync(x => x.ID.Equals(item.ID));
			if (itm != null)
			{
				itm.Name = item.Name;
				await Context.SaveChangesAsync();
			}
			return item;
		}

		public async Task<bool> AddDefaults()
		{
			var metal = new ProductGroup("Metallverarbeitung");
			var food = new ProductGroup("Food");
			var material = new ProductGroup("Baumaterial");

			var result = await AddItem(metal);
			result = await AddItem(food);
			result = await AddItem(material);

			return result;
		}
	}
}
