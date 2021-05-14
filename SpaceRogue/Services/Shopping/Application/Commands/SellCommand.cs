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

			var product = Engine.Galaxy.GetProduct(marketPlace, request.CatalogItemId.ToGuid());

			if(product==null)
			{
				_logger.LogError($"Corresponding product to catalog item {request.CatalogItemId} not found.");
				return false;
			}

			double price = Engine.Galaxy.GetDemandedProductPrice(marketPlace, request.CatalogItemId.ToGuid());

			var result = await _shipRepository.UnloadCargo(ship.ID, product.ID, request.Amount);
			if (!result)
			{
				_logger.LogError("Failed to load cargo.");
			}
			else
			{
				_logger.LogInformation($"{request.Amount} units loaded on {ship.Name}.");
			}

			var player = Engine.Galaxy.GetPlayer(ship.PlayerId);

			player.Credits = (decimal) await _playerRepsoitory.Deposit(player.ID, request.Amount * price);
			
			var eventMessage = new PlayerSoldProductIntegrationEvent
			{
				PlayerId = player.ID.ToString(),
				AccountBalance = (double)player.Credits,
				MarketPlaceId = marketPlace.ID.ToString()
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
