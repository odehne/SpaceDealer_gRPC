using System;
using System.Collections.Generic;
using static Cope.SpaceRogue.Infrastructure.Domain.Player;

namespace Cope.SpaceRogue.Shopping.API.Models
{
	public class PlayerModel
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public decimal Credits { get; set; }
		public PlayerTypes PlayerType { get; set; }
		public List<ShipModel> Fleet { get; set; }

		public PlayerModel()
		{
			PlayerType = PlayerTypes.Human;
			Fleet = new List<ShipModel>();
		}
	}
}