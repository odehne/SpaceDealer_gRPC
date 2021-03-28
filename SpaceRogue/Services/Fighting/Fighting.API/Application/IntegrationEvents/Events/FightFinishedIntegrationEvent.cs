using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Ship.API.Application.IntegrationEvents.Events
{
	public class FightFinishedIntegrationEvent : IntegrationEvent
	{
		public string AttackerId { get; set; }
		public string DefenderId { get; set; }

		public FightFinishedIntegrationEvent(string attackerId, string defenderId)
		{
			AttackerId = attackerId;
			DefenderId = defenderId;
		}
	}

}
