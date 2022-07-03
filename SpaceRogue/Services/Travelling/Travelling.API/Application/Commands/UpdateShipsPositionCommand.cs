using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Travelling.API.Application.IntegrationEvents;
using Cope.SpaceRogue.Travelling.API.Repositories;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.API.Application.Commands
{


    public class UpdateShipsPositionCommand : IRequest<bool>
	{
		[DataMember]
		public Guid ShipId { get; set; }

		[DataMember]
		public int CurrentPosX { get; set; }

		[DataMember]
		public int CurrentPosY { get; set; }

		[DataMember]
		public int CurrentPosZ { get; set; }

		public UpdateShipsPositionCommand(Guid shipId, int currentX, int currentY, int currentZ)
		{
			ShipId = shipId;
			CurrentPosX = currentX;
			CurrentPosY = currentY;
			CurrentPosZ = currentZ;
		}
	}

	public class UpdateShipsPositionCommandHandler : IRequestHandler<UpdateShipsPositionCommand, bool>
	{
		private readonly ILogger<UpdateShipsPositionCommandHandler> _logger;
		private readonly IShipRepository _repo;
		private readonly IPlanetRepository _planetRepo;
		private readonly IEventBus _eventBus;


		public UpdateShipsPositionCommandHandler(IShipRepository repo, IPlanetRepository planetRepo, ILogger<UpdateShipsPositionCommandHandler> logger, IEventBus eventBus)
		{
			_repo = repo;
			_logger = logger;
			_planetRepo = planetRepo;
			_eventBus = eventBus;
		}

		public async Task<bool> Handle(UpdateShipsPositionCommand request, CancellationToken cancellationToken)
		{
			var ship = await _repo.GetItem(request.ShipId);
			ship.CurrentPosX = request.CurrentPosX;
			ship.CurrentPosY = request.CurrentPosY; 
			ship.CurrentPosZ = request.CurrentPosZ;

			var planet = await _planetRepo.GetPlanetByPosition(ship.CurrentPosX, ship.CurrentPosY, ship.CurrentPosZ);

			if(planet != null)
            {
				var eventMessage = new JourneyReachedDestinationntegrationEvent
				{
					ShipId = ship.Id.ToString(),
					TargetPosX = planet.PosX,	
					TargetPosY = planet.PosY,	
					TargetPosZ = planet.PosZ,
					TargetObjectName = planet.Name
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
			}

			return await _repo.UpdatePosition(ship);
		}

	}
}
