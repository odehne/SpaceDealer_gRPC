using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Travelling.API.Application.Commands;
using Cope.SpaceRogue.Travelling.API.Models;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API.Application.IntegrationEvents
{

	public class ShipSuccessfullyEscapedIntegrationEvent : IntegrationEvent
	{
		public string ShipId { get; set; }
		public int CurrentPosX { get; set; }
		public int CurrentPosY { get; set; }
		public int CurrentPosZ { get; set; }

		public ShipSuccessfullyEscapedIntegrationEvent(string shipId, int currentPosX, int currentPosY, int currentPosZ)
		{
			ShipId = shipId;
			CurrentPosX = currentPosX;
			CurrentPosY = currentPosY;
			CurrentPosZ = currentPosZ;
		}

		public class ShipFeldIntegrationEventHandler : IIntegrationEventHandler<ShipSuccessfullyEscapedIntegrationEvent>
		{

			private readonly IMediator _mediator;
			private readonly IEventBus _eventBus;
			private readonly ILogger<ShipFeldIntegrationEventHandler> _logger;

			public ShipFeldIntegrationEventHandler(IMediator mediator, IEventBus eventBus, ILogger<ShipFeldIntegrationEventHandler> logger)
			{
				_mediator = mediator;
				_eventBus = eventBus;
				_logger = logger;
			}

			public async Task Handle(ShipSuccessfullyEscapedIntegrationEvent @event)
			{
				_logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
				var targetSector = new Position(@event.CurrentPosX + 1, @event.CurrentPosY + 1, @event.CurrentPosZ +1);
				var startJourneyCommand = new StartJourneyCommand(@event.ShipId, (int)targetSector.X, (int)targetSector.Y, (int)targetSector.Z);
				await _mediator.Send(startJourneyCommand);
			}
		}
	}

}