
using Cope.SpaceRogue.Infrastructure.Domain;
using System.Collections.Generic;

namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
    public class PlayerDto
	{
		public string ID { get; private set; }
		public string Name { get; private set; }
	
		public IEnumerable<string> ShipIds { get; private set; }

        public PlayerDto(string playerId, string playerName, IEnumerable<string> shipIds)
        {
            ID = playerId;
            Name = playerName;
            ShipIds = shipIds;
        }

		public static PlayerDto MapToDto(Player player)
		{
			var ids = new List<string>();
			foreach (var ship in player.Fleet)
			{
				ids.Add(ship.ID.ToString());
			}
			return new PlayerDto(player.ID.ToString(), player.Name, ids);
		}
    }
}
