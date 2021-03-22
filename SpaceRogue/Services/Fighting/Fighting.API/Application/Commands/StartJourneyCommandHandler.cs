using MediatR;
using Microsoft.Extensions.Logging;
using Ship.API.Application.IntegrationEvents;
using Ship.API.Application.IntegrationEvents.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ship.API.Application.Commands
{
	public class StartJourneyCommandHandler : IRequestHandler<StartJourneyCommand, bool>
	{
		private readonly IMediator _mediator;
		private readonly IShipInteractionIntegrationEventService _shipInteractionIntegrationEventService;
		private readonly ILogger<StartJourneyCommandHandler> _logger;

		public StartJourneyCommandHandler(IMediator mediator, 
			IShipInteractionIntegrationEventService shipInteractionIntegrationEventService, 
			ILogger<StartJourneyCommandHandler> logger)
		{
			_mediator = mediator;
			_shipInteractionIntegrationEventService = shipInteractionIntegrationEventService;
			_logger = logger;
		}

		public async Task<bool> Handle(StartJourneyCommand request, CancellationToken cancellationToken)
		{
			var journeyStartedIntegrationEvent = new JourneyStartedIntegrationEvent(request.ShipId);
			await _shipInteractionIntegrationEventService.AddAndSaveEventAsync(journeyStartedIntegrationEvent);


		}
	}
}
