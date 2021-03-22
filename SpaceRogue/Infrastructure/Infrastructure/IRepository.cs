using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Infrastructure.Repository
{
	public interface IRepository<T>
	{
		
		IPersister<T> Persister { get; set; }

		Task<T> GetItem(string itemId);

		Task<List<T>> Read();
		
		Task<bool> Save();

	}

}