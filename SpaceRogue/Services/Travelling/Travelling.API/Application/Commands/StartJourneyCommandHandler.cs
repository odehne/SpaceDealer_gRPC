using Cope.SpaceRogue.Travelling.API.Application.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using Cope.SpaceRogue.Travelling.API.Application.IntegrationEvents.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API.Application.Commands
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

			return true;
		}
	}
}
