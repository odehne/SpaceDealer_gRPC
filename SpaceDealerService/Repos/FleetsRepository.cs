using SpaceDealerModels.Units;
using System.Collections.Generic;

namespace SpaceDealerService.Repos
{
	public class FleetsRepository
	{
		public string DbPath { get; set; }

		public FleetsRepository(string dbPath)
		{
			DbPath = dbPath;
		}
		public List<DbShip> GetShips(string playerId)
		{
			var lst = new List<Ship>();

			return lst;
		}

		public Ship GetShip(string id)
		{
			var p = new Ship();

			return p;
		}

		public void SaveShip(string playerId, string shipId)
		{

		}
	}
}
