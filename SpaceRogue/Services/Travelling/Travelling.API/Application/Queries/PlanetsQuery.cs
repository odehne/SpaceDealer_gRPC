using Cope.SpaceRogue.Travelling.API.Models;
using Cope.SpaceRogue.Travelling.API;
using Cope.SpaceRogue.Travelling.API.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Travelling.Application.Queries
{

    public class PlanetsQuery : IRequest<IEnumerable<PlanetModel>> { }

    public class PlanetsQueryHandler : IRequestHandler<PlanetsQuery, IEnumerable<PlanetModel>>
    {
        private readonly IPlanetRepository _repository;

        public PlanetsQueryHandler(IPlanetRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<PlanetModel>> Handle(PlanetsQuery request, CancellationToken cancellationToken)
        {
            {
                var itms = await _repository.GetItems();
                var model = new List<PlanetModel>();

                foreach (var itm in itms)
                {
                    model.Add(AutoMap.Mapper.Map<PlanetModel>(itm));
                }

                return model;
            }
        }
    }
  
}
