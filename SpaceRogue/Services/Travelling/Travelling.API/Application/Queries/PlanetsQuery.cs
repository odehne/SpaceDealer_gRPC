using Cope.SpaceRogue.Travelling.API.Models;
using Cope.SpaceRogue.Travelling.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cope.SpaceRogue.Infrastructure;

namespace Cope.SpaceRogue.Travelling.Application.Queries
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
                var itms = await _repository.GetItems();
                var model = new List<PlanetModel>();

                foreach (var itm in itms)
                {
                    model.Add(new PlanetModel
                    {
                        PlanetId = itm.ID,
                        Name = itm.Name,
                        Sector = new Position(itm.PosX, itm.PosY, itm.PosZ)
                    });
                }

                return model;
            }
        }
    }
  
}
