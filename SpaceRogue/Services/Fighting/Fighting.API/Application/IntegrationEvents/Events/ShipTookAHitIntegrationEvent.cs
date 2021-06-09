using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events
{

	public class ShipTookAHitIntegrationEvent : IntegrationEvent
	{
		public string FightId { get; set; }
		public string AttackerId { get; set; }
		public string DefenderId { get; set; }

		public ShipTookAHitIntegrationEvent(string fightId, string attackerId, string defenderId)
		{
			FightId = fightId;
			AttackerId = attackerId;
			DefenderId = defenderId;
		}

		public class ShipTookAHitIntegrationEventHandler : IIntegrationEventHandler<ShipTookAHitIntegrationEvent>
		{
			private readonly IEventBus _eventBus;
			private readonly ILogger<ShipTookAHitIntegrationEventHandler> _logger;

			public ShipTookAHitIntegrationEventHandler(IEventBus eventBus, ILogger<ShipTookAHitIntegrationEventHandler> logger)
			{
				_eventBus = eventBus;
				_logger = logger;
			}

			public async Task Handle(ShipTookAHitIntegrationEvent @event)
			{
				_logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
				_eventBus.Publish(@event);
				await Task.CompletedTask;
			}
		}
	}

}