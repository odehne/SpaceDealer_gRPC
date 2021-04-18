using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Cope.SpaceRogue.Galaxy.API.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Cope.SpaceRogue.Infrastructure;


namespace Cope.SpaceRogue.Galaxy.API.Application.Commands
{
	public class AddCatalogCommand : IRequest<CatalogDto>
	{
		[DataMember]
		public string CatalogId { get; private set; }
		[DataMember]
		public string MarketPlaceId { get; private set; }
		[DataMember]
		public List<CatalogItemDto> CatalogItems { get; private set; }

		public AddCatalogCommand(string catalogId, string marketPlaceId, List<CatalogItemDto> catalogItems)
		{
			CatalogId = catalogId;
			MarketPlaceId = marketPlaceId;
			CatalogItems = catalogItems;
		}
	}

	public class AddCatalogCommandHandler : IRequestHandler<AddCatalogCommand, CatalogDto>
	{
		private readonly MarketPlaceRepository _repository;
		private readonly IMediator _mediator;

		public AddCatalogCommandHandler(IMediator mediator, MarketPlaceRepository repository)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CatalogDto> Handle(AddCatalogCommand request, CancellationToken cancellationToken)
		{
			var marketId = "";
			var market = await _repository.GetItem(request.MarketPlaceId.ToGuid());
			if (market.ProductOfferings.ID == request.CatalogId.ToGuid())
			{
				marketId = market.ProductOfferings.ID.ToString();
				foreach (var item in request.CatalogItems)
				{
					market.ProductOfferings.CatalogItems.Add(new SpaceRogue.Infrastructure.Domain.CatalogItem { ID = item.ID.ToGuid() });
				}
			}
			if (market.ProductDemands.ID == request.CatalogId.ToGuid())
			{
				marketId = market.ProductOfferings.ID.ToString();
				foreach (var item in request.CatalogItems)
				{
					market.ProductDemands.CatalogItems.Add(new SpaceRogue.Infrastructure.Domain.CatalogItem { ID = item.ID.ToGuid() });
				}
			}
			if (string.IsNullOrEmpty(marketId))
				throw new ArgumentException("Market not found.");

			return new CatalogDto(request.CatalogId, request.CatalogItems);
		}

	}
}
