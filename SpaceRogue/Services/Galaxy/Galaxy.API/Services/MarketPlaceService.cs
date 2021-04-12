using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Proto;
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

				return new AddMarketPlaceReply { MarketPlaceId = data.MarketPlaceId, Message = "OK" };
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

		private static CatalogDTO MapToCatalogDTO(AddCatalogRequest catalogRequest)
		{
			var catDto = new CatalogDTO
			{
				CatalogId = catalogRequest.CatalogId
			};
			var items = new List<CatalogItemDTO>();

			foreach (var item in catalogRequest.CatalogItems)
			{
				items.Add(new CatalogItemDTO(item.CatalogItemId, item.ProductId, item.Title, item.Price));
			}

			catDto.CatalogItems = items;
			return catDto;
		}

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
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

        public override Task<GetProductGroupsReply> GetProductGroups(GetProductGroupsRequest request, ServerCallContext context)
        {
			
            return base.GetProductGroups(request, context);
        }

        public override Task<GetProductGroupReply> GetProductGroup(GetProductGroupRequest request, ServerCallContext context)
        {
            return base.GetProductGroup(request, context);
        }
    }
}
