using SpaceDealer.Enums;
using SpaceDealerModels.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
	public class Players : ConcurrentDictionary<string, DbPlayer>
	{
	    public DbPlayer AddPlayer(DbPlayer player)
		{
			var p = GetPlayerByName(player.Name);
			if (p != null)
				return p;

			var firstShip = new DbShip(player.Name + "'s Ship", player, player.Parent.Features.GetFeatureSet(new FeatureNames[] { FeatureNames.SignalRange_1 }), player.Fleet);
			player.Fleet.AddShip(firstShip);

            if (TryAdd(player.Id, player))
			{
				foreach (var ship in player.Fleet.Values)
				{
					ship.StartCruise(ship.CurrentPlanet, player.GetRandomDiscoveredPlanet());
				}
			}
			else
			{
                throw new Exception($"Player with id {player.Id} already exists.");
            }
            return player;
		}

        public DbPlayer GetPlayerById(string id)
        {
			var gotValue = TryGetValue(id, out var player);

			if(!gotValue)
				throw new Exception($"Player with id {id} not found.");

			return player;
        }

        public DbPlayer GetPlayerByName(string name)
		{
			var player = Values.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return player;
		}

		public override string ToString()
		{
			var ret = "";
			foreach (var player in this)
			{
				ret += player.ToString() + "\n";
			}
			return ret.TrimEnd('\n');
		}

	}
}
