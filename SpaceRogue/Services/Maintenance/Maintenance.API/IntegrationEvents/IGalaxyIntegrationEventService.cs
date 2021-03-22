using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.API.IntegrationEvents
{
	public interface IGalaxyIntegrationEventService
	{
		Task SaveEventAndGalaxyChangesAsync(IntegrationEvent evt);

		Task PublishThroughEventBusAsync(IntegrationEvent evt);
	}

}
