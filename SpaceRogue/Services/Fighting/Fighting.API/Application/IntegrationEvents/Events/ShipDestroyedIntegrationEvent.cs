using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events
{

	public class ShipDownIntegrationEvent : IntegrationEvent
	{
		public Guid FightId { get; set; }
		public Guid AttackerId { get; set; }
		public Guid DefenderId { get; set; }

		public ShipDownIntegrationEvent(Guid requestId, Guid attackerId, Guid defenderId)
		{
			FightId = requestId;
			AttackerId = attackerId;
			DefenderId = defenderId;
		}

		public class ShipDownIntegrationEventHandler : IIntegrationEventHandler<ShipDownIntegrationEvent>
		{
			private readonly IEventBus _eventBus;
			private readonly ILogger<ShipDownIntegrationEventHandler> _logger;

			public ShipDownIntegrationEventHandler(IEventBus eventBus, ILogger<ShipDownIntegrationEventHandler> logger)
			{
				_eventBus = eventBus;
				_logger = logger;
			}

			public async Task Handle(ShipDownIntegrationEvent @event)
			{
				_logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
				_eventBus.Publish(@event);
				await Task.CompletedTask;
			}
		}
	}

}