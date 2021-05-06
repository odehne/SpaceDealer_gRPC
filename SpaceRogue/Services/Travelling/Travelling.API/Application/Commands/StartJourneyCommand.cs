using Cope.SpaceRogue.InfraStructure;
using Cope.SpaceRogue.Travelling.API.Application.IntegrationEvents;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API.Application.Commands
{

	public class StartJourneyCommand : IRequest<bool>
	{
		[DataMember]
		public string ShipId { get; set; }

		[DataMember]
		public int TargetPosX { get; set; }

		[DataMember]
		public int TargetPosY { get; set; }

		[DataMember]
		public int TargetPosZ { get; set; }

		public StartJourneyCommand()
		{

		}

		public StartJourneyCommand(string shipId, int targetX, int targetY, int targetZ)
		{
			ShipId = shipId;
			TargetPosX = targetX;
			TargetPosY = targetY;
			TargetPosZ = targetZ;
		}
	}
	public class StartJourneyCommandHandler : IRequestHandler<StartJourneyCommand, bool>
	{
		private readonly IMediator _mediator;
		private readonly ILogger<StartJourneyCommandHandler> _logger;
		private readonly IEventBus _eventBus;

		public StartJourneyCommandHandler(IMediator mediator, ILogger<StartJourneyCommandHandler> logger, IEventBus eventBus)
		{
			_mediator = mediator;
			_logger = logger;
			_eventBus = eventBus;
		}

		public async Task<bool> Handle(StartJourneyCommand request, CancellationToken cancellationToken)
		{

			var theShip = Engine.Galaxy.Ships.FirstOrDefault(x => x.ShipId.Equals(request.ShipId.ToGuid()));
			var targetPosition = new Position(request.TargetPosX, request.TargetPosY, request.TargetPosZ);
			var targetPlanet = Engine.Galaxy.Planets.FirstOrDefault(x => x.Sector.Equals(targetPosition));
			var targetShip = Engine.Galaxy.Ships.FirstOrDefault(x => x.CurrentSector.Equals(targetPosition));

			if (targetPlanet != null)
			{
				_logger.LogInformation($"Travel to planet [{targetPlanet.Name}] started.");
			}
			if (targetShip != null)
			{
				_logger.LogInformation($"Rescue missing to ship [{targetShip.Name}] started.");
			}

			theShip.TargetSector = targetPosition;

			var eventMessage = new JourneyStartedIntegrationEvent
				{
					ShipId = request.ShipId,
					TargetPosX = request.TargetPosX,
					TargetPosY = request.TargetPosY,
					TargetPosZ = request.TargetPosZ
				};

			try
			{
				_eventBus.Publish(eventMessage);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}", eventMessage.Id, Program.AppName);

				throw;
			}
			return true;
		}
	}
}
