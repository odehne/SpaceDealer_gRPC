using Cope.SpaceRogue.InfraStructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Infrastructure.Repository
{
	public class FileStoragePersister : IPersister<List<Entity>>
	{
		public Task<List<List<Entity>>> Read()
		{
			throw new NotImplementedException();
		}

		public Task<bool> Write(List<List<Entity>> items)
		{
			throw new NotImplementedException();
		}
	}
}
