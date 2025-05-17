using SpaceDealer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
	public class Players : List<DbPlayer>
	{
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, DbCoordinates newPosition, DbShip ship, DbPlayer player);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, DbShip ship, DbPlayer player, DbCoordinates newPosition);

		public DbPlayer AddPlayer(DbPlayer player)
		{
			var p = GetPlayerById(player.Id);
			if (p != null)
				return p;

			Add(player);
			player.Arrived += Player_Arrived;
			player.Interrupted += Player_Interrupted;

            foreach (var ship in player.Fleet)
            {
                ship.StartCruise(ship.CurrentPlanet, ship.Cruise.Destination);
            }

            return player;
		}

		private void Player_Interrupted(InterruptionType interruptionType, string message, DbShip ship, DbPlayer player, DbCoordinates newPosition)
		{
			Interrupted?.Invoke(interruptionType, message, ship, player, newPosition);
		}

		private void Player_Arrived(string message, DbCoordinates newPosition, DbShip ship, DbPlayer player)
		{
			Arrived?.Invoke(message, newPosition, ship, player);
		}

        public DbPlayer GetPlayerById(string id)
        {
            return this.FirstOrDefault(x => x.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        }

        public DbPlayer GetPlayerByName(string name)
		{
			var p = this.FirstOrDefault(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
			if (p != null)
			{
				p.Fleet.Clear();
                return p;
            }
            return null;
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
