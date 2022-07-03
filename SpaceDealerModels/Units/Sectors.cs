using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
    public class Sectors : List<Sector>
    {
		public Sector GetSector(DbCoordinates position)
		{
			return this.FirstOrDefault(x => x.Position.Equals(position));
		}

		public void AddShip(DbCoordinates position, string shipId)
        {
			var s = GetSector(position);
			if (s != null)
       			s.AddShip(shipId);
			else
				Add(new Sector(position, shipId, null, null));
		}
		public void AddPlanet(DbCoordinates position, string planetId)
		{
			var s = GetSector(position);
			if (s != null)
				s.AddPlanet(planetId);
			else
				Add(new Sector(position, null, planetId, null));
		}

		public void AddPlayer(DbCoordinates position, string playerId)
		{
			var s = GetSector(position);
			if (s != null)
				s.AddPlayer(playerId);
            else
        		Add(new Sector(position, null, null, playerId));
			
		}
	}
}
