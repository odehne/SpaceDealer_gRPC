using Cope.SpaceRogue.Shopping.API.Models;
using Cope.SpaceRogue.Shopping.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cope.SpaceRogue.Infrastructure;
using System.Runtime.Serialization;
using Cope.SpaceRogue.Shopping.API;

namespace Cope.SpaceRogue.Shopping.Application.Commands
{
    public class MarketPlaceQuery : IRequest<MarketPlaceModel>
    {
        [DataMember]
        public string MarketPlaceId { get; private set; }

        public MarketPlaceQuery(string marketPlaceId)
        {
            MarketPlaceId = marketPlaceId;
        }
    }

    public class MarketPlaceQueryHandler : IRequestHandler<MarketPlaceQuery, MarketPlaceModel>
    {
        private readonly IMarketPlaceRepository _repository;

        public MarketPlaceQueryHandler(IMarketPlaceRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<MarketPlaceModel> Handle(MarketPlaceQuery request, CancellationToken cancellationToken)
        {
            var market = await _repository.GetItem(request.MarketPlaceId.ToGuid());
            var model = new MarketPlaceModel();

            if (market != null)
			{
                model.ID = market.ID;
                model.Name = market.Name;
                model.ProductDemands = CatalogModel.MapToDto(market.ProductDemands);
                model.ProductOfferings = CatalogModel.MapToDto(market.ProductOfferings);
            }

            return model;
        }
    }

}
