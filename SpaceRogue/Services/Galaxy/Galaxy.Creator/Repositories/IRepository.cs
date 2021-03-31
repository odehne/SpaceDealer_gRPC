using Galaxy.API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Repositories
{
	public interface IRepository<T>
	{
		GalaxyDbContext Context { get; }
		List<T> GetItems();
		T GetItem(Guid id);
		T GetItemByName(string name);
		T UpdateItem(T item);
		void AddItem(T item);
		void DeleteItem(T item);
		void DeleteMany(Guid id);

	}
}
