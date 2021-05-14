using Cope.SpaceRogue.Infrastructure;
using Cope.SpaceRogue.Shopping.API.Application.IntegrationEvents;
using Cope.SpaceRogue.Shopping.API.Repositories;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Shopping.API.Application.Commands
{

	public class BuyCommand : IRequest<bool>
	{
		[DataMember]
		public string ShipId { get; set; }

		[DataMember]
		public string CatalogItemId { get; set; }

		[DataMember]
		public double Amount { get; set; }


		public BuyCommand()
		{

		}

		public BuyCommand(string shipId, string itemId, double amount)
		{
			ShipId = shipId;
			CatalogItemId = itemId;
			Amount = amount;
		}
	}
	public class BuyCommandHandler : IRequestHandler<BuyCommand, bool>
	{
		private readonly IMediator _mediator;
		private readonly ILogger<SellCommandHandler> _logger;
		private readonly IEventBus _eventBus;
		private readonly IPlayerRepository _playerRepsoitory;
		private readonly IShipRepository _shipRepository;

		public BuyCommandHandler(IMediator mediator, ILogger<SellCommandHandler> logger, IEventBus eventBus, IPlayerRepository playerRepo, IShipRepository shipRepo)
		{
			_mediator = mediator;
			_logger = logger;
			_eventBus = eventBus;
			_playerRepsoitory = playerRepo;
			_shipRepository = shipRepo;
		}

		public async Task<bool> Handle(BuyCommand request, CancellationToken cancellationToken)
		{
			//TODO: Add sell handling here 

			//var theShip = Engine.Galaxy.Ships.FirstOrDefault(x => x.ShipId.Equals(request.ShipId.ToGuid()));
			//var targetPosition = new Position(request.TargetPosX, request.TargetPosY, request.TargetPosZ);
			//var targetPlanet = Engine.Galaxy.Planets.FirstOrDefault(x => x.Sector.Equals(targetPosition));
			//var targetShip = Engine.Galaxy.Ships.FirstOrDefault(x => x.CurrentSector.Equals(targetPosition));

			//if (targetPlanet != null)
			//{
			//	_logger.LogInformation($"Travel to planet [{targetPlanet.Name}] started.");
			//}
			//if (targetShip != null)
			//{
			//	_logger.LogInformation($"Rescue missing to ship [{targetShip.Name}] started.");
			//}

			//theShip.TargetSector = targetPosition;

			//var eventMessage = new JourneyStartedIntegrationEvent
			//	{
			//		ShipId = request.ShipId,
			//		TargetPosX = request.TargetPosX,
			//		TargetPosY = request.TargetPosY,
			//		TargetPosZ = request.TargetPosZ
			//	};

			//try
			//{
			//	_eventBus.Publish(eventMessage);
			//}
			//catch (Exception ex)
			//{
			//	_logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}", eventMessage.Id, Program.AppName);

			//	throw;
			//}
			return true;
		}
	}
}
