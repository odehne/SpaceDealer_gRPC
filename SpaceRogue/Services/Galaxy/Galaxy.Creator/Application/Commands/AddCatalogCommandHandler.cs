using Cope.SpaceRogue.Galaxy.Creator.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Galaxy.Creator.Application.Commands
{
	public class AddCatalogCommandHandler : IRequestHandler<AddCatalogCommand, CatalogDTO>
	{
		private readonly MarketPlaceRepository _repository;
		private readonly IMediator _mediator;

		public AddCatalogCommandHandler(IMediator mediator, MarketPlaceRepository repository)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<CatalogDTO> Handle(AddCatalogCommand request, CancellationToken cancellationToken)
		{
			var marketId = "";
			var market = await _repository.GetItem(request.MarketPlaceId.ToGuid());
			if(market.ProductOfferings.ID == request.CatalogId.ToGuid())
			{
				marketId = market.ProductOfferings.ID.ToString();
				foreach (var item in request.CatalogItems)
				{
					market.ProductOfferings.CatalogItems.Add(new Domain.CatalogItem { ID = item.CatalogItemId.ToGuid() });
				}
			}
			if (market.ProductDemands.ID == request.CatalogId.ToGuid())
			{
				marketId = market.ProductOfferings.ID.ToString();
				foreach (var item in request.CatalogItems)
				{
					market.ProductDemands.CatalogItems.Add(new Domain.CatalogItem { ID = item.CatalogItemId.ToGuid() });
				}
			}
			if (string.IsNullOrEmpty(marketId))
				throw new ArgumentException("Market not found.");

			return new CatalogDTO(request.CatalogId, request.CatalogItems);
		}

	}
}
