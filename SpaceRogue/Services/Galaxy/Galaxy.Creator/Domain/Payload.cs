using Cope.SpaceRogue.Galaxy.Creator.Domain;
using Cope.SpaceRogue.InfraStructure;
using System;

namespace Cope.SpaceRogue.Galaxy.Creator.Domain
{
	public class Payload : Entity
	{
		public Guid ID { get; set; }
		public Product Product { get; set; }
		public double Quantity { get; set; }

		protected override void EnsureValidState()
		{
			throw new System.NotImplementedException();
		}
	}
}