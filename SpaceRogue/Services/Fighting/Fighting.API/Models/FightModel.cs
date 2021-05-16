using System;

namespace Cope.SpaceRogue.Fighting.API.Models
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

	public class FightModel
	{
		public Guid ID { get; set; }
		public int RoundNumber { get; set; }
		public ShipModel Attacker { get; set; }
		public ShipModel Defender { get; set; }

		public FightModel()
		{
			ID = Guid.NewGuid();
		}

		public FightModel(int roundNumber, ShipModel attacker, ShipModel defender)
		{
			ID = Guid.NewGuid();
			RoundNumber = roundNumber;
			Attacker = attacker;
			Defender = defender;
		}

    }
}
