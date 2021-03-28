using Cope.SpaceRogue.InfraStructure;

namespace Cope.SpaceRogue.Maintenance.API.Domain
{
	public class Payload : Entity
	{
		public EntityId ProductId { get; set; }
		public double Quantity { get; set; }

		protected override void EnsureValidState()
		{
			throw new System.NotImplementedException();
		}
	}
}