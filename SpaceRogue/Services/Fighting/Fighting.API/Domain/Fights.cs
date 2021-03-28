using Cope.SpaceRogue.Fighting.API.Models;
using System.Collections.Generic;

namespace Fighting.API.Domain
{
	public class Fights
	{
		public List<Fight> Sessions { get; set; }

		public FightStates AddFight(ShipModel attacker, ShipModel defender)
		{
			var fight = new Fight { Attacker = attacker, Defender = defender, RoundNumber = 0 };
			Sessions.Add(fight);
			return FightStates.FightStarted;
		}

		public FightStates RemoveFight(ShipModel attacker, ShipModel defender)
		{
			var fight = new Fight { Attacker = attacker, Defender = defender, RoundNumber = 0 };
			Sessions.Add(fight);
			return FightStates.FightStarted;
		}
	}
}
