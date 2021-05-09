using System;

namespace Cope.SpaceRogue.Infrastructure.Domain
{
	public class Payload : Entity
	{
		public Guid ID { get; set; }
		public virtual Product Product { get; set; }
		public double Quantity { get; set; }

		protected override void EnsureValidState()
		{
			throw new System.NotImplementedException();
		}
	}
}