using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events
{
	
	public class ShipAttackedIntegrationEvent : IntegrationEvent
    {
        public string ShipId { get; set; }

        public ShipAttackedIntegrationEvent(string shipId)
		{
			ShipId = shipId;
		}
	
		public class ShipAttackedIntegrationEventHandler : IIntegrationEventHandler<ShipAttackedIntegrationEvent>
		{
			private readonly IEventBus _eventBus;
			private readonly ILogger<ShipAttackedIntegrationEventHandler> _logger;
	
			public ShipAttackedIntegrationEventHandler(IEventBus eventBus, ILogger<ShipAttackedIntegrationEventHandler> logger)
			{
				_eventBus = eventBus;
				_logger = logger;
			}

			public async Task Handle(ShipAttackedIntegrationEvent @event)
			{
				_logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
				_eventBus.Publish(@event);
				await Task.CompletedTask;
			}
		}
	}
}
