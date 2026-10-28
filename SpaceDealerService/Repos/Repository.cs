using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SpaceDealerService.Repos
{
	public abstract class Repository<T>
	{
		public SqlPersistor Parent { get; set; }

		public Repository(SqlPersistor parent)
		{
			Parent = parent;
		}

		public abstract ConcurrentDictionary<string, T> GetAll();

		public abstract ConcurrentDictionary<string, T> GetAll(string id);

		public abstract T GetItemById(string id);

        public abstract T GetItemByName(string name);

        public abstract string GetItemId(string name);

		public abstract void Save(T item);
	}
}
