﻿using Cope.SpaceRogue.Galaxy.API.Model;
using Cope.SpaceRogue.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.InfraStructure
{
	public class FileStoragePersister : IPersister<List<Planet>>
	{
		public Task<List<List<Planet>>> Read()
		{
			throw new NotImplementedException();
		}

		public Task<bool> Write(List<List<Planet>> items)
		{
			throw new NotImplementedException();
		}
	}
}
