using Cope.SpaceRogue.Fighting.API.Models;
using Fighting.API.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Ship.API.Application.IntegrationEvents;
using Ship.API.Application.IntegrationEvents.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ship.API.Application.Commands
{
	public class StartFightCommandHandler : IRequestHandler<StartFightCommand, bool>
	{
		private readonly IMediator _mediator;
		private readonly IFightInteractionIntegrationEventService _fightInteractionIntegrationEventService;
		private readonly ILogger<StartFightCommandHandler> _logger;

		public StartFightCommandHandler(IMediator mediator, 
			IFightInteractionIntegrationEventService shipInteractionIntegrationEventService, 
			ILogger<StartFightCommandHandler> logger)
		{
			_mediator = mediator;
			_fightInteractionIntegrationEventService = shipInteractionIntegrationEventService;
			_logger = logger;
		}

		public async Task<bool> Handle(StartFightCommand request, CancellationToken cancellationToken)
		{

			//var newFight = new ShipModel() {  }

			var journeyStartedIntegrationEvent = new FightStartedIntegrationEvent(Guid.NewGuid(), request.AttackerShipId, request.DefenderShipId);
			await _fightInteractionIntegrationEventService.AddAndSaveEventAsync(journeyStartedIntegrationEvent);

			return false;
		}
	}
}
