using Cope.SpaceRogue.Galaxy.API.Domain;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
    public class PlayerDTO
	{
		public string PlayerId { get; private set; }
		public string PlayerName { get; private set; }
	
		public IEnumerable<string> ShipIds { get; private set; }

        public PlayerDTO(string playerId, string playerName, IEnumerable<string> shipIds)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            ShipIds = shipIds;
        }

		public static PlayerDTO MapToDto(Player player)
		{
			var ids = new List<string>();
			foreach (var ship in player.Fleet)
			{
				ids.Add(ship.ID.ToString());
			}
			return new PlayerDTO(player.ID.ToString(), player.Name, ids);
		}
    }
}
