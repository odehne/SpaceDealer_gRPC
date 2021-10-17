using Cope.SpaceRogue.Infrastructure;
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

		[DataMember]
		public int CurrentPosX { get; set; }

		[DataMember]
		public int CurrentPosY { get; set; }

		[DataMember]
		public int CurrentPosZ { get; set; }

		public StartJourneyCommand()
		{

		}

		public StartJourneyCommand(string shipId, int currentX, int currentY, int currentZ, int targetX, int targetY, int targetZ)
		{
			ShipId = shipId;
			CurrentPosX = currentX;
			CurrentPosY = currentY;
			CurrentPosZ = currentZ;
			   
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

			var sourcePosition = new Position(request.CurrentPosX, request.CurrentPosY, request.CurrentPosZ);
			var destinationPosition = new Position(request.TargetPosX, request.TargetPosY, request.TargetPosZ);
			
			var theShip = Engine.Galaxy.Ships.FirstOrDefault(x => x.ShipId.Equals(request.ShipId.ToGuid()));
			var targetPlanet = Engine.Galaxy.Planets.FirstOrDefault(x => x.Sector.Equals(destinationPosition));
			var targetShip = Engine.Galaxy.Ships.FirstOrDefault(x => x.CurrentSector.Equals(destinationPosition));
			var targetObjectName = "";

			if (targetPlanet != null)
			{
				targetObjectName = targetPlanet.Name;
				_logger.LogInformation($"Travel to planet [{targetPlanet.Name}] started.");
			}

			if (targetShip != null)
			{
				_logger.LogInformation($"Rescue missing to ship [{targetShip.Name}] started.");
				targetObjectName = targetShip.Name;
			}

			theShip.TargetSector = destinationPosition;

			var journey = Engine.AddJourney(request.ShipId.ToGuid(), sourcePosition, destinationPosition, sourcePosition, Domain.DestinationTypes.Planet, theShip.Speed);
			var eventMessage = new JourneyStartedIntegrationEvent
				{
					ShipId = theShip.Name,
					TargetPosX = request.TargetPosX,
					TargetPosY = request.TargetPosY,
					TargetPosZ = request.TargetPosZ,
					TargetObjectName = targetPlanet.Name
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
