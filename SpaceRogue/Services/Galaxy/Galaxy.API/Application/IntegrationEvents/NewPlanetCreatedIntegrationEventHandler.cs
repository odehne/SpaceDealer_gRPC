﻿using Cope.SpaceRogue.Galaxy.API;
using Cope.SpaceRogue.Galaxy.API.Domain.Events;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.API.Application.IntegrationEvents
{
	public class PlanetAddedIntegrationEventHandler : IIntegrationEventHandler<PlanetAddedIntegrationEvent>
	{
		private readonly IEventBus _eventBus;
		private readonly ILogger<PlanetAddedIntegrationEventHandler> _logger;

		public PlanetAddedIntegrationEventHandler(IEventBus eventBus, ILogger<PlanetAddedIntegrationEventHandler> logger)
		{
			_eventBus = eventBus;
			_logger = logger;
		}

		public async Task Handle(PlanetAddedIntegrationEvent @event)
		{
			_logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
			_eventBus.Publish(@event);
			await Task.CompletedTask;
		}
	}

}