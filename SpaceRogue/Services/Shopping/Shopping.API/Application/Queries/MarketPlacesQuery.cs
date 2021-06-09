using Cope.SpaceRogue.Shopping.API.Models;
using Cope.SpaceRogue.Shopping.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Shopping.Application.Queries
{
	public class MarketPlacesQuery : IRequest<List<MarketPlaceModel>> { }

    public class MarketPlaceQueryHandler : IRequestHandler<MarketPlacesQuery, List<MarketPlaceModel>>
    {
        private readonly IMarketPlaceRepository _repository;

        public MarketPlaceQueryHandler(IMarketPlaceRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<List<MarketPlaceModel>> Handle(MarketPlacesQuery request, CancellationToken cancellationToken)
        {
            var markets = await _repository.GetItems();

            var lst = new List<MarketPlaceModel>();

			foreach (var market in markets)
			{
                var model = new MarketPlaceModel();

                if (market != null)
                {
                    model.ID = market.ID;
                    model.Name = market.Name;
                    model.ProductDemands = CatalogModel.MapToDto(market.ProductDemands);
                    model.ProductOfferings = CatalogModel.MapToDto(market.ProductOfferings);
                }

                lst.Add(model);
            }

            return lst;
        }
    }

}
