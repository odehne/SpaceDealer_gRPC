using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API.Application.IntegrationEvents.Events
{

	public class ShieldsDownIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }
		public ShieldsDownIntegrationEvent(string shipId)
		{
			ShipId = shipId;
		}


		public class ShieldsDownIntegrationEventHandler : IIntegrationEventHandler<ShieldsDownIntegrationEvent>
		{
			private readonly IEventBus _eventBus;
			private readonly ILogger<ShieldsDownIntegrationEventHandler> _logger;

			public ShieldsDownIntegrationEventHandler(IEventBus eventBus, ILogger<ShieldsDownIntegrationEventHandler> logger)
			{
				_eventBus = eventBus;
				_logger = logger;
			}

			public async Task Handle(ShieldsDownIntegrationEvent @event)
			{
				_logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
				_eventBus.Publish(@event);
				await Task.CompletedTask;
			}
		}
	}

}