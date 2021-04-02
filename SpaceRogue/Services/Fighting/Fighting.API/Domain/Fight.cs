using Cope.SpaceRogue.Fighting.API.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fighting.API.Domain
{
	public enum FightStates
	{
		FightStarted,
		Hit,
		AttackMissed,
		ShipTookAHit,
		TargetFled,
		TargetDestroyed,
		ShipDestroyed, //no shields, not hull + final hit
		ShieldsCritical, // one before shields destroyed
		ShieldsDown, // shields destroyed
		HullCritical, // one before destroyed
		FightFinished
	}

	public class Fight
	{
		public int RoundNumber { get; set; }
		public ShipModel Attacker { get; set; }
		public ShipModel Defender { get; set; }

		public Fight()
		{

		}

		public Fight(int roundNumber, ShipModel attacker, ShipModel defender)
		{
			RoundNumber = roundNumber;
			Attacker = attacker;
			Defender = defender;
		}

    }
}
