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
	public class FightStartedIntegrationEventHandler : IIntegrationEventHandler<FightStartedIntegrationEvent>
	{
		private readonly IMediator _mediator;
		private readonly ILogger<FightStartedIntegrationEventHandler> _logger;

        public FightStartedIntegrationEventHandler(
           IMediator mediator,
           ILogger<FightStartedIntegrationEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Integration event handler which starts a fight
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the 
        /// game.api once it has recieved a start a fight request from the attacking ship.
        /// </param>
        /// <returns></returns>
        public async Task Handle(FightStartedIntegrationEvent @event)
		{
			using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
			{
				_logger.LogInformation($"----- Handling integration event: {@event.Id} at {Program.AppName} - ({@event})");

				var result = false;

                if (@event.AttackerId != Guid.Empty)
                {
                    using (LogContext.PushProperty("IdentifiedCommandId", @event.RequestId))
                    {
                        var startFightCommand = new StartFightCommand(@event.AttackerId, @event.DefenderId);

                        var requestStartFight = new IdentifiedCommand<StartFightCommand, bool>(startFightCommand, @event.RequestId);

                        _logger.LogInformation(
                            $"----- Sending command: {requestStartFight.GetGenericTypeName()} - {nameof(requestStartFight.Id)}: {requestStartFight.Id} ({requestStartFight})");

                        result = await _mediator.Send(requestStartFight);

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
