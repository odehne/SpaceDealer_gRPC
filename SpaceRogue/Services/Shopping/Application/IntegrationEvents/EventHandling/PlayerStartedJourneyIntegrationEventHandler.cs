using Cope.SpaceRogue.Services.Ship.API;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Ship.API.Application.Commands;
using Ship.API.Application.IntegrationEvents.Events;
using System;
using System.Threading.Tasks;

namespace Ship.API.Application.IntegrationEvents.EventHandling
{
	public class PlayerStartedJourneyIntegrationEventHandler : IIntegrationEventHandler<PlayerStartedJourneyIntegrationEvent>
	{
		private readonly IMediator _mediator;
		private readonly ILogger<PlayerStartedJourneyIntegrationEventHandler> _logger;

        public PlayerStartedJourneyIntegrationEventHandler(
           IMediator mediator,
           ILogger<PlayerStartedJourneyIntegrationEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Integration event handler which starts a journey
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the 
        /// game.api once it has recieved a start journey request from the player for that ship.
        /// </param>
        /// <returns></returns>
        public async Task Handle(PlayerStartedJourneyIntegrationEvent @event)
		{
			using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
			{
				_logger.LogInformation($"----- Handling integration event: {@event.Id} at {Program.AppName} - ({@event})");

				var result = false;

                if (@event.RequestId != Guid.Empty)
                {
                    using (LogContext.PushProperty("IdentifiedCommandId", @event.RequestId))
                    {
                        var startJourneyCommand = new StartJourneyCommand(@event.ShipId, @event.TargetX, @event.TargetY, @event.TargetZ);

                        var requestStartJourney = new IdentifiedCommand<StartJourneyCommand, bool>(startJourneyCommand, @event.RequestId);

                        _logger.LogInformation(
                            $"----- Sending command: {requestStartJourney.GetGenericTypeName()} - {nameof(requestStartJourney.Id)}: {requestStartJourney.Id} ({requestStartJourney})");

                        result = await _mediator.Send(requestStartJourney);

                        if (result)
                        {
                            _logger.LogInformation("----- CreateOrderCommand suceeded - RequestId: {RequestId}", @event.RequestId);
                        }
                        else
                        {
                            _logger.LogWarning("CreateOrderCommand failed - RequestId: {RequestId}", @event.RequestId);
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", @event);
                }
            }
		}
	}
}
