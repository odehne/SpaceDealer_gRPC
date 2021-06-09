using Cope.SpaceRogue.Shopping.API.Models;
using Cope.SpaceRogue.Shopping.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cope.SpaceRogue.Infrastructure;

namespace Cope.SpaceRogue.Shopping.Application.Queries
{

	public class PlanetsQuery : IRequest<List<PlanetModel>> { }

    public class PlanetsQueryHandler : IRequestHandler<PlanetsQuery, List<PlanetModel>>
    {
        private readonly IPlanetRepository _repository;

        public PlanetsQueryHandler(IPlanetRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<List<PlanetModel>> Handle(PlanetsQuery request, CancellationToken cancellationToken)
        {
            {
                var planets = await _repository.GetItems();
                var lst = new List<PlanetModel>();

                foreach (var planet in planets)
                {
                    var model = new PlanetModel
                    {
                        ID = planet.ID,
                        Name = planet.Name,
                        Sector = new Position(planet.PosX, planet.PosY, planet.PosZ),
                        Market = new MarketPlaceModel()
                    };
                    if (planet.Market != null)
                    {
                        model.Market.ID = planet.Market.ID;
                        model.Market.ID = planet.Market.ID;
                        model.Market.Name = planet.Market.Name;
                        model.Market.ProductDemands = CatalogModel.MapToDto(planet.Market.ProductDemands);
                        model.Market.ProductOfferings = CatalogModel.MapToDto(planet.Market.ProductOfferings);
                    }
                    lst.Add(model);
                }
                
                return lst;
            }
        }
    }
  
}
