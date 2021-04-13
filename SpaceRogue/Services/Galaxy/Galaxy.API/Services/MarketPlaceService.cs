using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Proto;
using Galaxy.Creator.Application.Commands;
using Google.Protobuf.Collections;
using Grpc.Core;
using MediatR;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.API.Services
{
	public class MarketPlaceService : MarketPlacesService.MarketPlacesServiceBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<MarketPlaceService> _logger;

		public MarketPlaceService(IMediator mediator, ILogger<MarketPlaceService> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		public async override Task<AddMarketPlaceReply> AddMarketPlace(AddMarketPlaceRequest addMarketPlaceRequest, ServerCallContext context)
		{
			_logger.LogInformation("Begin grpc call from method {Method} for ordering get order draft {CreateOrderDraftCommand}", context.Method, addMarketPlaceRequest);
			_logger.LogTrace(
				"----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
				addMarketPlaceRequest.GetGenericTypeName(),
				nameof(addMarketPlaceRequest.MarketPlaceId),
				addMarketPlaceRequest.MarketPlaceId, 
				addMarketPlaceRequest);

			var command = MapToCreateMarketPlaceCommand(addMarketPlaceRequest);

			var data = await _mediator.Send(command);

			if (data != null)
			{
				context.Status = new Status(StatusCode.OK, $" market place {addMarketPlaceRequest} does exist");

				return new AddMarketPlaceReply { MarketPlaceId = data.ID, Message = "OK" };
			}
			else
			{
				context.Status = new Status(StatusCode.NotFound, $" marketplace {addMarketPlaceRequest} does not exist");
			}

			return new AddMarketPlaceReply();
		}

		public override Task<DeleteMarketPlaceReply> DeleteMarketPlace(DeleteMarketPlaceRequest request, ServerCallContext context)
		{
			return base.DeleteMarketPlace(request, context);
		}

		public override Task<UpdateMarketPlaceReply> UpdateMarketPlace(UpdateMarketPlaceRequest request, ServerCallContext context)
		{
			return base.UpdateMarketPlace(request, context);
		}

		private static AddMarketPlaceCommand MapToCreateMarketPlaceCommand(AddMarketPlaceRequest addMarketPlaceRequest)
		{
			return new AddMarketPlaceCommand
			{
				PlanetId = addMarketPlaceRequest.PlanetId,
				MarketPlaceName = addMarketPlaceRequest.Name,
				MarketPlaceId = addMarketPlaceRequest.MarketPlaceId,
				Demands = MapToCatalogDTO(addMarketPlaceRequest.Demands),
				Offerings = MapToCatalogDTO(addMarketPlaceRequest.Offerings)
			};
		}

		private static CatalogDto MapToCatalogDTO(AddCatalogRequest catalogRequest)
		{
			var catDto = new CatalogDto
			{
				ID = catalogRequest.CatalogId
			};
			var items = new List<CatalogItemDto>();

			foreach (var item in catalogRequest.CatalogItems)
			{
				items.Add(new CatalogItemDto(item.CatalogItemId, item.ProductId, item.Title, item.Price));
			}

			catDto.CatalogItems = items;
			return catDto;
		}

        public override Task<AddProductReply> AddProduct(AddProductRequest request, ServerCallContext context)
        {
            return base.AddProduct(request, context);
        }

        public async override Task<AddProductGroupReply> AddProductGroup(AddProductGroupRequest request, ServerCallContext context)
        {
			var command = new AddProductGroupCommand(request.ProductGroupId, request.Name);
            var rply = await _mediator.Send(command);
			return new AddProductGroupReply { OK = true };
		}

        public override Task<AddCatalogItemReply> AddCatalogItem(AddCatalogItemRequest request, ServerCallContext context)
        {
            return base.AddCatalogItem(request, context);
        }

        public override Task<AddCatalogReply> AddCatalog(AddCatalogRequest request, ServerCallContext context)
        {
            return base.AddCatalog(request, context);
        }

        public async override Task<GetProductGroupsReply> GetProductGroups(Empty request, ServerCallContext context)
        {
			var groups = await _mediator.Send(new ProductGroupsQuery());
			var dtos = new GetProductGroupsReply();
			foreach (var group in groups)
			{
				dtos.ProductGroups.Add(new GetProductGroupReply { Name = group.Name, ProductGroupId = group.ID });
			}
			return dtos;
        }

        public async override Task<GetProductGroupReply> GetProductGroup(GetProductGroupRequest request, ServerCallContext context)
        {
			var group = await _mediator.Send(new ProductGroupQuery(request.ProductGroupId));
			return new GetProductGroupReply { Name = group.Name, ProductGroupId = group.ID };
		}

		public async override Task<GetProductsReply> GetProducts(Empty request, ServerCallContext context)
		{
			var prds = await _mediator.Send(new ProductsQuery());
			var reply = new GetProductsReply();

			foreach (var prd in prds)
			{
				reply.Products.Add(new GetProductReply { Name = prd.Name, PricePerUnit = prd.PricePerUnit, ProductId = prd.ID, Rarity = prd.Rarity, SizeInUnits = prd.Capacity });
			}

			return reply;
		}

		public async override Task<GetProductReply> GetProduct(GetProductRequest request, ServerCallContext context)
		{
			var prd = await _mediator.Send(new ProductQuery(request.ProductId));
			return new GetProductReply { Name = prd.Name, PricePerUnit = prd.PricePerUnit, ProductId = prd.ID, Rarity = prd.Rarity, SizeInUnits = prd.Capacity };
		}
	}
}
