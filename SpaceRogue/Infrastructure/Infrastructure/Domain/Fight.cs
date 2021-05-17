using System;
using System.ComponentModel.DataAnnotations;

namespace Cope.SpaceRogue.Infrastructure.Domain
{
	public enum FightStates
	{
		FightStarted,
		Hit,
		Missed,
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
		public virtual Ship Attacker { get; set; }
		public virtual Ship Defender { get; set; }
		public FightStates State { get; set; }
		public int RoundNumber { get; set; }

		protected override void EnsureValidState()
		{
			throw new NotImplementedException();
		}
	}
}