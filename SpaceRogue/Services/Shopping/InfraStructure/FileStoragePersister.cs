using Cope.SpaceRogue.Infrastructure.Repository;
using Cope.SpaceRogue.Ship.API.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Ship.API.Infrastructure
{
	public class FileStoragePersister : IPersister<List<Freighter>>
	{
		public Task<List<List<Freighter>>> Read()
		{
			throw new NotImplementedException();
		}

		public Task<bool> Write(List<List<Freighter>> items)
		{
			throw new NotImplementedException();
		}
	}
}
