using Grpc.Core;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Cope.SpaceRogue.Shopping.API.Proto;
using System;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Shopping.API.Services
{
	public class ShoppingService : ShopService.ShopServiceBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<ShoppingService> _logger;
		private readonly IEventBus _eventBus;

		public ShoppingService(IMediator mediator, ILogger<ShoppingService> logger, IEventBus eventBus)
		{
			_mediator = mediator;
			_logger = logger;
			_eventBus = eventBus;
		}

		public override Task<BuyReply> Buy(BuyRequest request, ServerCallContext context)
		{
			return base.Buy(request, context);
		}

		public override Task<GetCatalogItemReply> GetCatalogItem(GetCatalogItemRequest request, ServerCallContext context)
		{
			return base.GetCatalogItem(request, context);
		}

		public override Task<GetCatalogItemsReply> GetCatalogItems(GetCatalogItemsRequest request, ServerCallContext context)
		{
			return base.GetCatalogItems(request, context);
		}

		public override Task<GetMarketPlaceReply> GetMarketPlace(GetMarketPlaceRequest request, ServerCallContext context)
		{
			return base.GetMarketPlace(request, context);
		}

		public override Task<GetMarketPlacesReply> GetMarketPlaces(ShoppingEmpty request, ServerCallContext context)
		{
			return base.GetMarketPlaces(request, context);
		}

		public override Task<SellReply> Sell(SellRequest request, ServerCallContext context)
		{
			return base.Sell(request, context);
		}
	}
}