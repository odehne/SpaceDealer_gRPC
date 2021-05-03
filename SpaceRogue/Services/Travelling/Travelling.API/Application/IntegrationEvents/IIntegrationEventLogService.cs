using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API.Application.IntegrationEvents
{
	public interface IIntegrationEventLogService
	{
		Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId);
		Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction);
		Task MarkEventAsPublishedAsync(Guid eventId);
		Task MarkEventAsInProgressAsync(Guid eventId);
		Task MarkEventAsFailedAsync(Guid eventId);
	}
}
