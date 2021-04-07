using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Application.IntegrationEvents
{
	public interface ICreationIntegrationEventService
	{
		Task PublishEventsThroughEventBusAsync(Guid transactionId);
		Task AddAndSaveEventAsync(IntegrationEvent evt);
	}
}
