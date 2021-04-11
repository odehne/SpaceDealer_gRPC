using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Application.IntegrationEvents
{
	public class CreationIntegrationEventService : ICreationIntegrationEventService
	{
		private readonly IEventBus _eventBus;
		private readonly IIntegrationEventLogService _eventLogService;
		private readonly ILogger<CreationIntegrationEventService> _logger;

		public Task AddAndSaveEventAsync(IntegrationEvent evt)
		{
			throw new NotImplementedException();
		}

		public Task PublishEventsThroughEventBusAsync(Guid transactionId)
		{
			throw new NotImplementedException();
		}
	}
}
