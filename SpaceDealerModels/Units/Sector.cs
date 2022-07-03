using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
    public class Sector
    {
		[JsonProperty("position")]
		public DbCoordinates Position { get; set; }
		[JsonProperty("shipIds")]
		public List<string> ShipIds { get; set; }
		[JsonProperty("planetIds")]
		public List<string> PlanetIds { get; set; }
		[JsonProperty("playerIds")]
		public List<string> PlayerIds { get; set; }

        public Sector(DbCoordinates position)
        {
			Position = position;
            ShipIds = new List<string>();
            PlanetIds = new List<string>();
            PlayerIds = new List<string>();
        }

        public override string ToString()
        {
            return Position.ToString();
        }

        public Sector(DbCoordinates position, string shipId, string planetId, string playerId)
        {
            Position = position;
            ShipIds = new List<string>();
            PlanetIds = new List<string>();
            PlayerIds = new List<string>();

            if(shipId!=null)
                AddShip(shipId);
            if (playerId != null)
                AddPlayer(playerId);
            if (planetId != null)
                AddPlanet(planetId);
        }

        public string GetShipIds()
        {
            return string.Join(",", ShipIds);
        }

        public void AddPlayers(string playerIds)
        {
            if(string.IsNullOrEmpty(playerIds))
                return;
            if(!playerIds.Contains(','))
            {
                AddPlayer(playerIds);
            }
            else
            {
                var ids = playerIds.Split(',');
                foreach (var id in ids)
                {
                    AddPlayer(id);
                }
            }
        }

        public void AddShips(string shipIds)
        {
            if (string.IsNullOrEmpty(shipIds))
                return;
            if (!shipIds.Contains(','))
            {
                AddShip(shipIds);
            }
            else
            {
                var ids = shipIds.Split(',');
                foreach (var id in ids)
                {
                    AddShip(id);
                }
            }
        }

        public void AddPlanets(string planetIds)
        {
            if (string.IsNullOrEmpty(planetIds))
                return;
            if (!planetIds.Contains(','))
            {
                AddPlanet(planetIds);
            }
            else
            {
                var ids = planetIds.Split(',');
                foreach (var id in ids)
                {
                    AddPlanet(id);
                }
            }
        }

        public void AddPlayer(string playerId)
        {
            var id = PlayerIds.FirstOrDefault(p => p.Equals(playerId));
            if(id == null)
                PlayerIds.Add(playerId);
        }
        public void AddPlanet(string planetId)
        {
            var id = PlanetIds.FirstOrDefault(p => p.Equals(planetId));
            if (id == null)
                PlanetIds.Add(planetId);
        }
        public void AddShip(string shipId)
        {
            var id = ShipIds.FirstOrDefault(p => p.Equals(shipId));
            if (id == null)
                ShipIds.Add(shipId);
        }
    }
}
