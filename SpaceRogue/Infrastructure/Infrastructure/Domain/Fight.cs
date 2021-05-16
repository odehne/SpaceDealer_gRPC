using System;
using System.ComponentModel.DataAnnotations;

namespace Cope.SpaceRogue.Infrastructure.Domain
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

	public class Fight : Entity
	{
		[Key]
		public Guid ID { get; set; }
		public Ship Attacker { get; set; }
		public Ship Defender { get; set; }
		public FightStates State { get; set; }
		public int RoundNumber { get; set; }

		protected override void EnsureValidState()
		{
			throw new NotImplementedException();
		}
	}
}