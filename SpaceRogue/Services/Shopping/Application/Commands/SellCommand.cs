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

	public class SellCommand : IRequest<bool>
	{
		[DataMember]
		public string ShipId { get; set; }

		[DataMember]
		public string CatalogItemId { get; set; }

		[DataMember]
		public string MarketPlaceId { get; set; }

		[DataMember]
		public double Amount { get; set; }


		public SellCommand()
		{

		}

		public SellCommand(string shipId, string marketPlaceId, string itemId, double amount)
		{
			ShipId = shipId;
			CatalogItemId = itemId;
			MarketPlaceId = marketPlaceId;
			Amount = amount;
		}
	}
	public class SellCommandHandler : IRequestHandler<SellCommand, bool>
	{
		private readonly IMediator _mediator;
		private readonly ILogger<SellCommandHandler> _logger;
		private readonly IEventBus _eventBus;
		private readonly IPlayerRepository _playerRepsoitory;
		private readonly IPlanetRepository _planetsRepository;
		private readonly IShipRepository _shipRepository;

		public SellCommandHandler(IMediator mediator, ILogger<SellCommandHandler> logger, IEventBus eventBus, IPlayerRepository playerRepo, IShipRepository shipRepo, IPlanetRepository planetRepo)
		{
			_mediator = mediator;
			_logger = logger;
			_eventBus = eventBus;
			_playerRepsoitory = playerRepo;
			_planetsRepository = planetRepo;
			_shipRepository = shipRepo;
		}

		public async Task<bool> Handle(SellCommand request, CancellationToken cancellationToken)
		{
			//TODO: Add sell handling here

			var ship = Engine.Galaxy.GetShip(request.ShipId.ToGuid());

			if (!ship.EnoughLoadedToSell(request.CatalogItemId, request.Amount))
			{
				_logger.LogError("Not enough loaded to sell requested amount.");
				return false;
			}

			var marketPlace = Engine.Galaxy.GetMarketPlace(request.MarketPlaceId.ToGuid());
			if (marketPlace==null)
			{
				_logger.LogError($"Market place not found [{request.MarketPlaceId}].");
				return false;
			}

			double price = Engine.Galaxy.GetPrice(marketPlace, request.CatalogItemId.ToGuid());
			if (marketPlace == null)
			{
				_logger.LogError($"Market place not found [{request.MarketPlaceId}].");
				return false;
			}



			_shipRepository.Sell()

			//var transactionId = Engine.Galaxy.Sell(request.MarketPlaceId.ToGuid(), request.CatalogItemId.ToGuid(), request.Amount, request.ShipId);
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
