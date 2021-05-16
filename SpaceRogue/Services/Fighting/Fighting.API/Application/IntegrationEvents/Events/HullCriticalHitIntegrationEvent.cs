using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events
{

	public class HullCriticalHitIntegrationEvent : IntegrationEvent
	{
		public Guid FightId { get; set; }
		public Guid AttackerId { get; set; }
		public Guid DefenderId { get; set; }

		public HullCriticalHitIntegrationEvent(Guid requestId, Guid attackerId, Guid defenderId)
		{
			FightId = requestId;
			AttackerId = attackerId;
			DefenderId = defenderId;
		}

		public class HullCriticalHitIntegrationEventHandler : IIntegrationEventHandler<HullCriticalHitIntegrationEvent>
		{
			private readonly IEventBus _eventBus;
			private readonly ILogger<HullCriticalHitIntegrationEventHandler> _logger;

			public HullCriticalHitIntegrationEventHandler(IEventBus eventBus, ILogger<HullCriticalHitIntegrationEventHandler> logger)
			{
				_eventBus = eventBus;
				_logger = logger;
			}

			public async Task Handle(HullCriticalHitIntegrationEvent @event)
			{
				_logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
				_eventBus.Publish(@event);
				await Task.CompletedTask;
			}
		}
	}

}