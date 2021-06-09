using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events
{

	public class ShipDestroyedIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }
		public ShipDestroyedIntegrationEvent(string shipId)
		{
			ShipId = shipId;
		}

		public class ShipDestroyedIntegrationEventHandler : IIntegrationEventHandler<ShipDestroyedIntegrationEvent>
		{
			private readonly IEventBus _eventBus;
			private readonly ILogger<ShipDestroyedIntegrationEventHandler> _logger;

			public ShipDestroyedIntegrationEventHandler(IEventBus eventBus, ILogger<ShipDestroyedIntegrationEventHandler> logger)
			{
				_eventBus = eventBus;
				_logger = logger;
			}

			public async Task Handle(ShipDestroyedIntegrationEvent @event)
			{
				_logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
				_eventBus.Publish(@event);
				await Task.CompletedTask;
			}
		}
	}

}