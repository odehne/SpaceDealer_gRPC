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

		public abstract List<T> GetAll();

		public abstract List<T> GetAll(string id);

		public abstract T GetItem(string name, string id);

		public abstract string GetItemId(string name);

		public abstract void Save(T item);
	}
}
