using Cope.SpaceRogue.Galaxy.API.InfraStructure;

namespace Cope.SpaceRogue.Galaxy.API.Domain
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