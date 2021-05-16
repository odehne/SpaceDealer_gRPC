using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events
{
	
	public class FightStartedIntegrationEvent : IntegrationEvent
    {
        public string FightId { get; set; }
        public string AttackerId { get; set; }
        public string DefenderId { get; set; }

        public FightStartedIntegrationEvent(string requestId, string attackerId, string defenderId)
		{
            FightId = requestId;
			AttackerId = attackerId;
            DefenderId = defenderId;
		}
	
		public class FightStartedIntegrationEventHandler : IIntegrationEventHandler<FightStartedIntegrationEvent>
		{
			private readonly IEventBus _eventBus;
			private readonly ILogger<FightStartedIntegrationEventHandler> _logger;
	
			public FightStartedIntegrationEventHandler(IEventBus eventBus, ILogger<FightStartedIntegrationEventHandler> logger)
			{
				_eventBus = eventBus;
				_logger = logger;
			}

			public async Task Handle(FightStartedIntegrationEvent @event)
			{
				_logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
				_eventBus.Publish(@event);
				await Task.CompletedTask;
			}
		}
	}
}
