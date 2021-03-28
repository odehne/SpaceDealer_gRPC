using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public class AttackMissedIntegrationEvent : IntegrationEvent
	{
		public string AttackerId { get; set; }
		public string DefenderId { get; set; }

		public AttackMissedIntegrationEvent(string attackerId, string defenderId)
		{
			AttackerId = attackerId;
			DefenderId = defenderId;
		}
	}

}
