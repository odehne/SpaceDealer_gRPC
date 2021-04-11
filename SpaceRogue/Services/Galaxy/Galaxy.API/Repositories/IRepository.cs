using Cope.SpaceRogue.Galaxy.API;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Repositories
{
	public interface IRepository<T>
	{
		GalaxyDbContext Context { get; }
		Task<List<T>> GetItems();
		Task<T> GetItem(Guid id);
		Task<T> GetItemByName(string name);
		Task<T> UpdateItem(T item);
		Task<bool> AddItem(T item);
		Task<bool> DeleteItem(T item);

	}
}
