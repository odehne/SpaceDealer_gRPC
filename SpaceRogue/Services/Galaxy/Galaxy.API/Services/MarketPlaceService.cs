using Cope.SpaceRogue.Galaxy.API.Application.Commands;
using Cope.SpaceRogue.Galaxy.API.Proto;
using Galaxy.Creator.Application.Commands;
using Grpc.Core;
using MediatR;
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

		public async override Task<OkReply> DeleteProduct(GetProductRequest request, ServerCallContext context)
		{
			var result = await _mediator.Send(new DeleteProductComand(request.Id));
			return new OkReply { Ok = result };
		}

		public async override Task<OkReply> DeleteProductGroup(GetProductGroupRequest request, ServerCallContext context)
		{
			var result = await _mediator.Send(new DeleteProductGroupComand(request.Id));
			return new OkReply { Ok = result };
		}

		public async override Task<OkReply> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
		{
			var result = await _mediator.Send(AutoMap.Mapper.Map<UpdateProductCommand>(request));
			return new OkReply { Ok = true };
		}

		public async override Task<AddMarketPlaceReply> AddMarketPlace(AddMarketPlaceRequest addMarketPlaceRequest, ServerCallContext context)
		{
			_logger.LogInformation("Begin grpc call from method {Method} for ordering get order draft {CreateOrderDraftCommand}", context.Method, addMarketPlaceRequest);
			var command = new AddMarketPlaceCommand(addMarketPlaceRequest.Id, 
										  addMarketPlaceRequest.Name, 
										  addMarketPlaceRequest.Id,
										  AutoMap.Mapper.Map<CatalogDto>(addMarketPlaceRequest.Offerings),
										  AutoMap.Mapper.Map<CatalogDto>(addMarketPlaceRequest.Demands));

			var data = await _mediator.Send(command);

			if (data != null)
			{
				context.Status = new Status(StatusCode.OK, $" market place {addMarketPlaceRequest} does exist");

				return new AddMarketPlaceReply { Id = data.ID, Message = "OK" };
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

        public async override Task<OkReply> AddProduct(AddProductRequest request, ServerCallContext context)
        {
			var command = new AddProductCommand(request.Id, request.Name, request.GroupId, request.PricePerUnit, request.Capacity, request.Rarity);
			var rply = await _mediator.Send(command);
			return new OkReply { Ok = true };
		}

		public async override Task<OkReply> AddProductGroup(AddProductGroupRequest request, ServerCallContext context)
        {
			var command = new AddProductGroupCommand(request.Id, request.Name);
            var rply = await _mediator.Send(command);
			return new OkReply { Ok = true };
		}

        public async override Task<OkReply> AddCatalogItem(AddCatalogItemRequest request, ServerCallContext context)
        {
			var command = new AddCatalogItemCommand(request.CatalogId, request.ProductId, request.Title, request.PricePercentDelta);
			var rply = await _mediator.Send(command);
			return new OkReply { Ok = true };
		}

        public override Task<OkReply> AddCatalog(AddCatalogRequest request, ServerCallContext context)
        {
            return base.AddCatalog(request, context);
        }

        public async override Task<GetProductGroupsReply> GetProductGroups(Empty request, ServerCallContext context)
        {
			var groups = await _mediator.Send(new ProductGroupsQuery());
			var dtos = new GetProductGroupsReply();
			dtos.ProductGroups.Add(AutoMap.Mapper.Map<IList<GetProductGroupReply>>(groups));
			return dtos;
        }

        public async override Task<GetProductGroupReply> GetProductGroup(GetProductGroupRequest request, ServerCallContext context)
        {
			var group = await _mediator.Send(new ProductGroupQuery(request.Id));
			return AutoMap.Mapper.Map<GetProductGroupReply>(group);
		}

		public async override Task<GetProductsReply> GetProducts(Empty request, ServerCallContext context)
		{
			var prds = await _mediator.Send(new ProductsQuery());
			var reply = new GetProductsReply();
			reply.Products.Add(AutoMap.Mapper.Map<IList<GetProductReply>>(prds));
			return reply;
		}

		public async override Task<GetProductReply> GetProduct(GetProductRequest request, ServerCallContext context)
		{
			var prd = await _mediator.Send(new ProductQuery(request.Id));
			return AutoMap.Mapper.Map<GetProductReply>(prd);
		}

		public async override Task<GetCatalogItemReply> GetCatalogItem(GetCatalogItemRequest request, ServerCallContext context)
		{
			var item = await _mediator.Send(new CatalogItemQuery(request.CatalogId));
			return AutoMap.Mapper.Map<GetCatalogItemReply>(item);
		}

		public async override Task<GetCatalogItemsReply> GetCatalogItems(GetCatalogItemsRequest request, ServerCallContext context)
		{
			var items = await _mediator.Send(new CatalogItemsQuery());
			var reply = new GetCatalogItemsReply();
			reply.CatalogItems.Add(AutoMap.Mapper.Map<IList<GetCatalogItemReply>>(items));
			return reply;
		}
	}
}
