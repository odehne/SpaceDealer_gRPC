using SpaceDealer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceDealerModels.Units
{
	public class Players : List<Player>
	{
		public event ArrivedAtDestination Arrived;
		public delegate void ArrivedAtDestination(string message, Coordinates newPosition, Ship ship, Player player);
		public event JourneyInterrupted Interrupted;
		public delegate void JourneyInterrupted(InterruptionType interruptionType, string message, Ship ship, Player player, Coordinates newPosition);

		public Player AddPlayer(Player player)
		{
			var p = GetPlayerByName(player.Name);
			if (p != null)
				return p;

			Add(player);
			player.Arrived += Player_Arrived;
			player.Interrupted += Player_Interrupted;
			return player;
		}

		private void Player_Interrupted(InterruptionType interruptionType, string message, Ship ship, Player player, Coordinates newPosition)
		{
			Interrupted?.Invoke(interruptionType, message, ship, player, newPosition);
		}

		private void Player_Arrived(string message, Coordinates newPosition, Ship ship, Player player)
		{
			Arrived?.Invoke(message, newPosition, ship, player);
		}

		public Player GetPlayerByName(string name)
		{
			return this.FirstOrDefault(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
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
