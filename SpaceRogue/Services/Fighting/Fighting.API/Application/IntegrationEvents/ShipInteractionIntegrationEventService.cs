using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ship.API.Application.IntegrationEvents
{
	public interface IShipInteractionIntegrationEventService
	{
		Task PublishEventsThroughEventBusAsync(Guid transactionId);
		Task AddAndSaveEventAsync(IntegrationEvent evt);
	}
}
