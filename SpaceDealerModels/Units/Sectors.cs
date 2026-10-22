using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
    public class Sectors : ConcurrentDictionary<DbCoordinates, Sector>
    {
		public Sector GetSector(DbCoordinates position)
		{
			return this.TryGetValue(position, out Sector sector) ? sector : null;
		}

		public void AddShip(DbCoordinates position, string shipId)
        {
			var s = GetSector(position);
			if (s != null)
       			s.AddShip(shipId);
			else
				TryAdd(position, new Sector(position, shipId, null, null));
		}

        public void MoveShip(DbCoordinates oldPosition, DbCoordinates position, string shipId)
        {
            var s = GetSector(oldPosition);
            if (s != null)
                s.RemoveShip(shipId);
            s = GetSector(position);
            if (s != null)
                s.AddShip(shipId);
            else
                TryAdd(position, new Sector(position, shipId, null, null));
        }

        public void AddPlanet(DbCoordinates position, string planetId)
		{
			var s = GetSector(position);
			if (s != null)
				s.AddPlanet(planetId);
			else
				TryAdd(position, new Sector(position, null, planetId, null));
		}

		public void AddPlayer(DbCoordinates position, string playerId)
		{
			var s = GetSector(position);
			if (s != null)
				s.AddPlayer(playerId);
            else
        		TryAdd(position, new Sector(position, null, null, playerId));
			
		}

        public void MovePlayer(DbCoordinates oldPosition, DbCoordinates position, string shipId)
        {
            var s = GetSector(oldPosition);
            if (s != null)
                s.RemovePlayer(shipId);
            s = GetSector(position);
            if (s != null)
                s.AddPlayer(shipId);
            else
                TryAdd(position, new Sector(position, null, null, shipId));
        }
    }
}
