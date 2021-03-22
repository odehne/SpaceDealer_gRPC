using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Infrastructure.Repository
{
	public interface IPersister<T>
	{
		Task<List<T>> Read();
		Task<bool> Write(List<T> items);
	}

	
}